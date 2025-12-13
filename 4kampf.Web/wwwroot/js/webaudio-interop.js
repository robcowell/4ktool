// WebAudio Interop for 4kampf
window.webaudioInterop = {
    contexts: new Map(),
    
    init: function(contextId) {
        const audioContext = new (window.AudioContext || window.webkitAudioContext)();
        
        this.contexts.set(contextId, {
            audioContext: audioContext,
            source: null,
            gainNode: null,
            analyser: null,
            buffer: null,
            isPlaying: false,
            startTime: 0,
            pauseTime: 0,
            duration: 0,
            volume: 1.0,
            loop: false
        });
        
        return contextId;
    },
    
    loadAudio: function(contextId, audioUrl) {
        const ctx = this.contexts.get(contextId);
        if (!ctx) return Promise.reject('Context not found');
        
        return fetch(audioUrl)
            .then(response => response.arrayBuffer())
            .then(arrayBuffer => ctx.audioContext.decodeAudioData(arrayBuffer))
            .then(audioBuffer => {
                ctx.buffer = audioBuffer;
                ctx.duration = audioBuffer.duration;
                return ctx.duration;
            })
            .catch(error => {
                console.error('Error loading audio:', error);
                throw error;
            });
    },
    
    play: function(contextId) {
        const ctx = this.contexts.get(contextId);
        if (!ctx || !ctx.buffer) return;
        
        if (ctx.isPlaying) {
            this.pause(contextId);
            return;
        }
        
        const source = ctx.audioContext.createBufferSource();
        source.buffer = ctx.buffer;
        source.loop = ctx.loop;
        
        if (!ctx.gainNode) {
            ctx.gainNode = ctx.audioContext.createGain();
            ctx.gainNode.gain.value = ctx.volume;
        }
        
        if (!ctx.analyser) {
            ctx.analyser = ctx.audioContext.createAnalyser();
            ctx.analyser.fftSize = 2048;
        }
        
        source.connect(ctx.gainNode);
        ctx.gainNode.connect(ctx.analyser);
        ctx.analyser.connect(ctx.audioContext.destination);
        
        const offset = ctx.pauseTime;
        ctx.startTime = ctx.audioContext.currentTime - offset;
        source.start(0, offset);
        
        source.onended = () => {
            if (!ctx.loop) {
                ctx.isPlaying = false;
                ctx.pauseTime = 0;
            }
        };
        
        ctx.source = source;
        ctx.isPlaying = true;
        
        // Resume audio context if suspended
        if (ctx.audioContext.state === 'suspended') {
            ctx.audioContext.resume();
        }
    },
    
    pause: function(contextId) {
        const ctx = this.contexts.get(contextId);
        if (!ctx || !ctx.isPlaying) return;
        
        if (ctx.source) {
            ctx.pauseTime = this.getPosition(contextId);
            ctx.source.stop();
            ctx.source.disconnect();
            ctx.source = null;
        }
        
        ctx.isPlaying = false;
    },
    
    stop: function(contextId) {
        const ctx = this.contexts.get(contextId);
        if (!ctx) return;
        
        if (ctx.source) {
            ctx.source.stop();
            ctx.source.disconnect();
            ctx.source = null;
        }
        
        ctx.isPlaying = false;
        ctx.pauseTime = 0;
    },
    
    setPosition: function(contextId, time) {
        const ctx = this.contexts.get(contextId);
        if (!ctx) return;
        
        const wasPlaying = ctx.isPlaying;
        if (wasPlaying) {
            this.pause(contextId);
        }
        
        ctx.pauseTime = Math.max(0, Math.min(time, ctx.duration));
        
        if (wasPlaying) {
            this.play(contextId);
        }
    },
    
    getPosition: function(contextId) {
        const ctx = this.contexts.get(contextId);
        if (!ctx || !ctx.buffer) return 0;
        
        if (ctx.isPlaying && ctx.source) {
            return ctx.audioContext.currentTime - ctx.startTime;
        }
        
        return ctx.pauseTime;
    },
    
    getDuration: function(contextId) {
        const ctx = this.contexts.get(contextId);
        if (!ctx || !ctx.buffer) return 0;
        
        return ctx.duration;
    },
    
    setVolume: function(contextId, volume) {
        const ctx = this.contexts.get(contextId);
        if (!ctx) return;
        
        ctx.volume = Math.max(0, Math.min(1, volume));
        if (ctx.gainNode) {
            ctx.gainNode.gain.value = ctx.volume;
        }
    },
    
    getVolume: function(contextId) {
        const ctx = this.contexts.get(contextId);
        if (!ctx) return 1.0;
        
        return ctx.volume;
    },
    
    setLoop: function(contextId, loop) {
        const ctx = this.contexts.get(contextId);
        if (!ctx) return;
        
        ctx.loop = loop;
        if (ctx.source) {
            ctx.source.loop = loop;
        }
    },
    
    getEnvelopeData: function(contextId, numChannels) {
        const ctx = this.contexts.get(contextId);
        if (!ctx || !ctx.buffer) return null;
        
        const currentPos = this.getPosition(contextId);
        const sampleRate = ctx.buffer.sampleRate;
        const sampleIndex = Math.floor(currentPos * sampleRate / 256);
        
        const envelopeData = [];
        for (let i = 0; i < numChannels && i < ctx.buffer.numberOfChannels; i++) {
            const channelData = ctx.buffer.getChannelData(i);
            if (sampleIndex < channelData.length) {
                envelopeData.push(channelData[sampleIndex]);
            } else {
                envelopeData.push(0);
            }
        }
        
        return envelopeData;
    },
    
    cleanup: function(contextId) {
        const ctx = this.contexts.get(contextId);
        if (!ctx) return;
        
        this.stop(contextId);
        
        if (ctx.audioContext) {
            ctx.audioContext.close();
        }
        
        this.contexts.delete(contextId);
    }
};

