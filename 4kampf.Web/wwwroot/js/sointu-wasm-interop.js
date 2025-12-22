// Sointu WebAssembly Interop for 4kampf
// This module loads and manages the Sointu WASM module via Web Worker to prevent blocking the UI

window.sointuWasmInterop = {
    worker: null,
    audioContext: null,
    scriptProcessor: null,
    isInitialized: false,
    isPlaying: false,
    sampleRate: 44100,
    bufferSize: 4096,
    currentTime: 0,
    songData: null,
    envelopeData: null,
    numInstruments: 0,
    songDuration: 0,
    loop: false,
    pendingCallbacks: new Map(),
    messageId: 0,
    
    /**
     * Send a message to the worker and wait for response
     * @private
     */
    _sendToWorker(type, data) {
        return new Promise((resolve, reject) => {
            const id = ++this.messageId;
            const timeout = setTimeout(() => {
                this.pendingCallbacks.delete(id);
                reject(new Error(`Worker message timeout: ${type}`));
            }, 300000); // 5 minute timeout for long operations like compilation
            
            this.pendingCallbacks.set(id, (result, error) => {
                clearTimeout(timeout);
                if (error) {
                    reject(new Error(error));
                } else {
                    resolve(result);
                }
            });
            
            this.worker.postMessage({ type, data, id });
        });
    },
    
    /**
     * Initialize the Sointu WASM module via Web Worker
     * @param {string} wasmPath - Path to the Sointu WASM file
     * @returns {Promise<boolean>} - True if initialization succeeded
     */
    async init(wasmPath) {
        try {
            // Check if WebAssembly is supported
            if (typeof WebAssembly === 'undefined') {
                console.error('WebAssembly is not supported in this browser');
                return false;
            }
            
            // Check if Web Workers are supported
            if (typeof Worker === 'undefined') {
                console.error('Web Workers are not supported in this browser');
                return false;
            }
            
            // Create AudioContext for real-time synthesis
            this.audioContext = new (window.AudioContext || window.webkitAudioContext)({
                sampleRate: this.sampleRate
            });
            
            // Create Web Worker
            this.worker = new Worker('/js/sointu-wasm-worker.js');
            
            // Handle messages from worker
            this.worker.onmessage = (e) => {
                const { type, result, error, id, log } = e.data;
                
                // Forward log messages from worker to console (for progress bar updates)
                if (log) {
                    console.log(log);
                }
                
                const callback = this.pendingCallbacks.get(id);
                
                if (callback) {
                    this.pendingCallbacks.delete(id);
                    callback(result, error);
                } else {
                    console.warn('Received message from worker with no callback:', type, id);
                }
            };
            
            // Handle worker errors
            this.worker.onerror = (error) => {
                console.error('Worker error:', error);
            };
            
            // Initialize WASM in worker
            const initResult = await this._sendToWorker('init', { wasmPath });
            
            if (initResult && initResult.success) {
                this.isInitialized = true;
                console.log('Sointu WASM module loaded successfully in Web Worker');
                return true;
            } else {
                const errorMsg = initResult?.error || 'Unknown error';
                console.error('Failed to initialize WASM in worker:', errorMsg);
                return false;
            }
        } catch (error) {
            console.error('Error initializing Sointu WASM:', error);
            return false;
        }
    },
    
    /**
     * Load a Sointu YAML song file and compile it
     * @param {string} yamlContent - YAML song content
     * @returns {Promise<boolean>} - True if compilation succeeded
     */
    async loadSong(yamlContent) {
        if (!this.isInitialized) {
            console.error('WASM module not initialized');
            return false;
        }
        
        try {
            console.log('Compiling song in Web Worker (this may take 10-30 seconds)...');
            const result = await this._sendToWorker('compileSong', { yamlContent });
            
            if (result && result.success) {
                this.songData = result;
                this.numInstruments = result.numInstruments || 0;
                this.songDuration = result.duration || 0;
                
                // Store pre-rendered audio buffer (interleaved stereo Float32Array)
                // The buffer is transferred from the worker, so it's already a Float32Array
                if (result.audioBuffer) {
                    this.preRenderedBuffer = result.audioBuffer; // Float32Array with interleaved [L, R, L, R, ...]
                    console.log(`Pre-rendered audio buffer received: ${this.preRenderedBuffer.length} samples (${this.preRenderedBuffer.length / 2} stereo samples)`);
                } else {
                    console.warn('No audio buffer in compile result');
                }
                
                // Store envelope data
                if (result.envelopeData) {
                    this.envelopeData = result.envelopeData;
                }
                
                console.log(`Song loaded: ${this.numInstruments} instruments, duration: ${this.songDuration.toFixed(2)}s`);
                return true;
            } else {
                const errorMsg = result?.error || 'Unknown error';
                console.error('Song compilation failed:', errorMsg);
                return false;
            }
        } catch (error) {
            console.error('Error loading song:', error);
            return false;
        }
    },
    
    /**
     * Generate envelope data for shader synchronization
     * Note: Envelopes are now generated during song compilation in WASM
     * This method is kept for compatibility but may not be needed
     * @returns {Promise<void>}
     */
    async generateEnvelopes() {
        // Envelopes are generated during compileSong in the WASM module
        // No need to generate separately
        console.log('Envelope data will be available via getEnvelopeSync');
    },
    
    /**
     * Start real-time audio synthesis
     * @returns {Promise<boolean>} - True if playback started
     */
    async play() {
        if (!this.isInitialized || !this.songData || this.isPlaying) {
            return false;
        }
        
        try {
            // Create ScriptProcessorNode for real-time synthesis
            // Note: ScriptProcessorNode is deprecated but still widely supported
            // For better performance, consider using AudioWorkletNode (requires separate worklet file)
            this.scriptProcessor = this.audioContext.createScriptProcessor(
                this.bufferSize,
                0,
                2 // Stereo output
            );
            
            // Check if pre-rendered buffer is available
            if (!this.preRenderedBuffer) {
                console.error('No pre-rendered audio buffer available for playback');
                return false;
            }
            
            this.scriptProcessor.onaudioprocess = (e) => {
                if (!this.isPlaying || !this.songData || !this.preRenderedBuffer) {
                    return;
                }
                
                const leftChannel = e.outputBuffer.getChannelData(0);
                const rightChannel = e.outputBuffer.getChannelData(1);
                
                // Calculate sample index in pre-rendered buffer (interleaved stereo: [L, R, L, R, ...])
                // Each stereo sample is 2 float32 values, so we multiply by 2
                const startIndex = Math.floor(this.currentTime * this.sampleRate) * 2; // Index in interleaved buffer
                const endIndex = startIndex + (this.bufferSize * 2); // Need 2 values per sample
                
                if (startIndex < this.preRenderedBuffer.length) {
                    const actualEndIndex = Math.min(endIndex, this.preRenderedBuffer.length);
                    const numSamplesToRead = Math.floor((actualEndIndex - startIndex) / 2);
                    
                    // Copy interleaved samples directly from buffer
                    for (let i = 0; i < numSamplesToRead && i < this.bufferSize; i++) {
                        const bufferIndex = startIndex + (i * 2);
                        if (bufferIndex + 1 < this.preRenderedBuffer.length) {
                            const left = this.preRenderedBuffer[bufferIndex];
                            const right = this.preRenderedBuffer[bufferIndex + 1];
                            // Clamp to valid range [-1, 1]
                            leftChannel[i] = (typeof left === 'number' && !isNaN(left)) ? Math.max(-1, Math.min(1, left)) : 0;
                            rightChannel[i] = (typeof right === 'number' && !isNaN(right)) ? Math.max(-1, Math.min(1, right)) : 0;
                        } else {
                            leftChannel[i] = 0;
                            rightChannel[i] = 0;
                        }
                    }
                    
                    // Fill remaining buffer with silence if needed
                    for (let i = numSamplesToRead; i < this.bufferSize; i++) {
                        leftChannel[i] = 0;
                        rightChannel[i] = 0;
                    }
                } else {
                    // Past end of buffer - output silence
                    for (let i = 0; i < this.bufferSize; i++) {
                        leftChannel[i] = 0;
                        rightChannel[i] = 0;
                    }
                }
                
                // Update current time
                this.currentTime += this.bufferSize / this.sampleRate;
                
                // Check if we've reached the end
                if (this.songDuration && this.currentTime >= this.songDuration) {
                    if (this.loop) {
                        this.currentTime = 0;
                    } else {
                        this.stop();
                    }
                }
            };
            
            // Connect to audio output
            this.scriptProcessor.connect(this.audioContext.destination);
            
            // Resume audio context if suspended
            if (this.audioContext.state === 'suspended') {
                await this.audioContext.resume();
            }
            
            this.isPlaying = true;
            console.log('Sointu WASM playback started');
            return true;
        } catch (error) {
            console.error('Error starting playback:', error);
            return false;
        }
    },
    
    /**
     * Stop audio synthesis
     */
    stop() {
        if (this.scriptProcessor) {
            this.scriptProcessor.disconnect();
            this.scriptProcessor = null;
        }
        
        this.isPlaying = false;
        this.currentTime = 0;
    },
    
    /**
     * Pause audio synthesis
     */
    pause() {
        this.isPlaying = false;
    },
    
    /**
     * Get current playback position
     * @returns {number} - Current time in seconds
     */
    getPosition() {
        return this.currentTime;
    },
    
    /**
     * Set playback position
     * @param {number} time - Time in seconds
     */
    setPosition(time) {
        this.currentTime = Math.max(0, time);
    },
    
    /**
     * Get envelope sync data for current position
     * @param {number} numInstruments - Number of instruments to return
     * @returns {Float32Array|null} - Envelope values for each instrument
     */
    async getEnvelopeSync(numInstruments) {
        if (!this.songData) {
            return null;
        }
        
        // Call getEnvelopeSync via worker
        try {
            const envelopes = await this._sendToWorker('getEnvelopeSync', { time: this.currentTime });
            if (envelopes && Array.isArray(envelopes)) {
                // Convert to Float32Array and limit to requested number
                const result = new Float32Array(Math.min(numInstruments, envelopes.length));
                for (let i = 0; i < result.length; i++) {
                    result[i] = envelopes[i] || 0;
                }
                return result;
            }
        } catch (error) {
            console.warn('Error getting envelope sync:', error);
        }
        
        // Fallback: return zeros if no envelope data available
        return new Float32Array(numInstruments);
    },
    
    /**
     * Check if WASM module is available
     * @returns {boolean}
     */
    isAvailable() {
        return this.isInitialized && this.wasmInstance !== null;
    },
    
    /**
     * Cleanup resources
     */
    cleanup() {
        this.stop();
        
        if (this.audioContext) {
            this.audioContext.close();
            this.audioContext = null;
        }
        
        if (this.worker) {
            this.worker.terminate();
            this.worker = null;
        }
        
        this.songData = null;
        this.envelopeData = null;
        this.preRenderedBuffer = null;
        this.isInitialized = false;
    }
};

