// Web Worker for Sointu WASM - runs WASM in a background thread to prevent blocking the UI

// Import the WASM interop script (we'll need to adapt it for worker context)
importScripts('/js/wasm_exec.js');

let go = null;
let isInitialized = false;

// Intercept console.log to capture progress messages from Go
const originalConsoleLog = console.log;
console.log = function(...args) {
    // Call original console.log
    originalConsoleLog.apply(console, args);
    
    // Check if this is a progress message from Go
    const message = args.join(' ');
    const progressMatch = message.match(/Render progress:\s*(\d+)%/i) || 
                          message.match(/DEBUG: Render progress:\s*(\d+)%/i);
    if (progressMatch) {
        const percent = parseInt(progressMatch[1], 10);
        // Forward progress to main thread
        self.postMessage({ 
            type: 'progress', 
            percent: percent,
            message: `Rendering: ${percent}%`
        });
    } else if (message.includes('Starting song render') || message.includes('DEBUG: Starting song render')) {
        self.postMessage({ 
            type: 'progress', 
            percent: 0,
            message: 'Starting render...'
        });
    } else if (message.includes('Song render complete') || message.includes('DEBUG: Song render complete')) {
        self.postMessage({ 
            type: 'progress', 
            percent: 100,
            message: 'Render complete!'
        });
    }
};

// Initialize WASM module in the worker
async function initWasm(wasmPath) {
    try {
        if (typeof WebAssembly === 'undefined') {
            throw new Error('WebAssembly is not supported');
        }

        // Create Go WASM runtime
        if (typeof Go === 'undefined') {
            throw new Error('Go WASM runtime (wasm_exec.js) not loaded');
        }

        go = new Go();
        
        // Load WASM module
        const wasmModule = await WebAssembly.instantiateStreaming(
            fetch(wasmPath),
            go.importObject
        );
        
        // Run Go program - go.run() is synchronous and blocks forever (because Go program has select{})
        // But the Go program's main() runs synchronously, so exports are available immediately
        // We need to check for exports before go.run() blocks, or use setTimeout to check after main() runs
        
        // Start go.run() - it will block this worker thread, but that's OK (main thread won't block)
        // The Go program's main() function runs synchronously and exports functions immediately
        // Then it hits select{} which blocks forever
        
        // The issue: go.run() is async and awaits _exitPromise which never resolves
        // This blocks the worker thread, so setTimeout/microtasks can't run
        // Solution: Use go.run() but check for exports synchronously right after calling it
        // (without awaiting), since main() runs synchronously when run() is called
        
        return new Promise((resolve, reject) => {
            try {
                // Call go.run() - it's async but we don't await it
                // The key: main() runs synchronously when this._inst.exports.run() is called inside go.run()
                const runPromise = go.run(wasmModule.instance);
                
                // Immediately check for exports (synchronously, before go.run() blocks on await)
                // This works because main() runs synchronously when run() is called
                const compileSong = globalThis.compileSong || self.compileSong;
                
                if (typeof compileSong === 'function') {
                    isInitialized = true;
                    console.log('[Worker] compileSong function found immediately after go.run()!');
                    resolve({ success: true });
                    // Don't await runPromise - it will block forever, but exports are already available
                } else {
                    // Exports not found immediately - this shouldn't happen if main() ran
                    // But let's provide detailed debug info
                    const availableGlobal = Object.keys(globalThis).filter(k => typeof globalThis[k] === 'function');
                    const availableSelf = Object.keys(self).filter(k => typeof self[k] === 'function');
                    const allAvailable = [...new Set([...availableGlobal, ...availableSelf])];
                    const sampleKeys = Object.keys(globalThis).slice(0, 30);
                    const compileSongExists = ('compileSong' in globalThis) || ('compileSong' in self);
                    const compileSongType = typeof compileSong;
                    
                    console.error('[Worker] compileSong not found immediately. Debug info:', {
                        type: compileSongType,
                        exists: compileSongExists,
                        availableFunctions: allAvailable,
                        sampleKeys: sampleKeys
                    });
                    
                    reject(new Error(
                        `compileSong not found immediately after go.run(). ` +
                        `Type: ${compileSongType}, Exists: ${compileSongExists}. ` +
                        `Available functions: ${allAvailable.join(', ')}. ` +
                        `Sample keys: ${sampleKeys.join(', ')}`
                    ));
                }
            } catch (error) {
                reject(error);
            }
        });
    } catch (error) {
        return { success: false, error: error.message };
    }
}

// Handle messages from main thread
self.onmessage = async function(e) {
    const { type, data, id } = e.data;
    
    try {
        switch (type) {
            case 'init':
                const result = await initWasm(data.wasmPath);
                self.postMessage({ type: 'init', result, id });
                break;
                
            case 'compileSong':
                if (!isInitialized) {
                    self.postMessage({ 
                        type: 'compileSong', 
                        result: { success: false, error: 'WASM not initialized' },
                        id 
                    });
                    return;
                }
                
                // Call the exported compileSong function
                // In worker context, Go exports to globalThis (or self)
                const compileSong = globalThis.compileSong || self.compileSong;
                if (typeof compileSong === 'function') {
                    try {
                        // Send initial progress
                        self.postMessage({ 
                            type: 'progress', 
                            percent: 0,
                            message: 'Compiling song...'
                        });
                        
                        const compileResult = compileSong(data.yamlContent);
                        
                        // Convert JavaScript arrays to Float32Array for efficient transfer
                        if (compileResult && compileResult.success) {
                            // Convert audioBuffer from JS array to Float32Array (interleaved stereo)
                            if (compileResult.audioBuffer && Array.isArray(compileResult.audioBuffer)) {
                                compileResult.audioBuffer = new Float32Array(compileResult.audioBuffer);
                            }
                            
                            // Convert envelopeData from JS array to Float32Array
                            if (compileResult.envelopeData && Array.isArray(compileResult.envelopeData)) {
                                compileResult.envelopeData = new Float32Array(compileResult.envelopeData);
                            }
                        }
                        
                        // Transfer the buffers efficiently using Transferable objects
                        const transferList = [];
                        if (compileResult.audioBuffer && compileResult.audioBuffer.buffer) {
                            transferList.push(compileResult.audioBuffer.buffer);
                        }
                        if (compileResult.envelopeData && compileResult.envelopeData.buffer) {
                            transferList.push(compileResult.envelopeData.buffer);
                        }
                        
                        self.postMessage({ type: 'compileSong', result: compileResult, id }, transferList);
                    } catch (error) {
                        self.postMessage({ 
                            type: 'compileSong', 
                            result: { success: false, error: `compileSong error: ${error.message}` },
                            id 
                        });
                    }
                } else {
                    // Debug: list available functions and properties
                    const availableGlobal = Object.keys(globalThis).filter(k => typeof globalThis[k] === 'function');
                    const availableSelf = Object.keys(self).filter(k => typeof self[k] === 'function');
                    const allAvailable = [...new Set([...availableGlobal, ...availableSelf])];
                    
                    // Also check for compileSong as a property (not just function)
                    const compileSongProp = globalThis.compileSong || self.compileSong;
                    const compileSongType = typeof compileSongProp;
                    
                    // List some sample keys from globalThis for debugging
                    const sampleKeys = Object.keys(globalThis).slice(0, 30);
                    
                    self.postMessage({ 
                        type: 'compileSong', 
                        result: { 
                            success: false, 
                            error: `compileSong function not available (type: ${compileSongType}). Available functions: ${allAvailable.join(', ')}. Sample keys: ${sampleKeys.join(', ')}` 
                        },
                        id 
                    });
                }
                break;
                
            case 'renderSamples':
                if (!isInitialized) {
                    self.postMessage({ type: 'renderSamples', result: null, id });
                    return;
                }
                
                // Call the exported renderSamples function
                const renderSamples = globalThis.renderSamples || self.renderSamples;
                if (typeof renderSamples === 'function') {
                    const samples = renderSamples(data.startTime, data.numSamples);
                    self.postMessage({ type: 'renderSamples', result: samples, id });
                } else {
                    self.postMessage({ type: 'renderSamples', result: null, id });
                }
                break;
                
            case 'getEnvelopeSync':
                if (!isInitialized) {
                    self.postMessage({ type: 'getEnvelopeSync', result: null, id });
                    return;
                }
                
                // Call the exported getEnvelopeSync function
                const getEnvelopeSync = globalThis.getEnvelopeSync || self.getEnvelopeSync;
                if (typeof getEnvelopeSync === 'function') {
                    const envelopes = getEnvelopeSync(data.time);
                    self.postMessage({ type: 'getEnvelopeSync', result: envelopes, id });
                } else {
                    self.postMessage({ type: 'getEnvelopeSync', result: null, id });
                }
                break;
                
            case 'resetPlayback':
                const resetPlayback = globalThis.resetPlayback || self.resetPlayback;
                if (typeof resetPlayback === 'function') {
                    resetPlayback();
                }
                self.postMessage({ type: 'resetPlayback', result: true, id });
                break;
                
            default:
                self.postMessage({ type: 'error', error: `Unknown message type: ${type}`, id });
        }
    } catch (error) {
        self.postMessage({ type: 'error', error: error.message, stack: error.stack, id });
    }
};

// Intercept console.log from Go WASM and forward to main thread
// This allows progress messages to be intercepted by the main thread's console.log interceptor
const originalWorkerConsoleLog = console.log;
const originalWorkerConsoleError = console.error;
const originalWorkerConsoleWarn = console.warn;

console.log = function(...args) {
    originalWorkerConsoleLog.apply(console, args);
    const message = args.join(' ');
    // Forward to main thread so it can be intercepted for progress bar updates
    self.postMessage({ type: 'log', log: message });
};

console.error = function(...args) {
    originalWorkerConsoleError.apply(console, args);
    const message = args.join(' ');
    self.postMessage({ type: 'log', log: message });
};

console.warn = function(...args) {
    originalWorkerConsoleWarn.apply(console, args);
    const message = args.join(' ');
    self.postMessage({ type: 'log', log: message });
};
