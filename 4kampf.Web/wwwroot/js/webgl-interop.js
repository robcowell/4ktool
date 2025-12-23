// WebGL Interop for 4kampf
window.webglInterop = {
    contexts: new Map(),
    
    init: function(canvasId) {
        const canvas = document.getElementById(canvasId);
        if (!canvas) {
            console.error('Canvas not found:', canvasId);
            return null;
        }
        
        const gl = canvas.getContext('webgl2') || canvas.getContext('webgl');
        if (!gl) {
            console.error('WebGL not supported');
            return null;
        }
        
        const contextId = canvasId;
        this.contexts.set(contextId, {
            gl: gl,
            canvas: canvas,
            shaders: new Map(),
            programs: new Map(),
            textures: new Map(),
            framebuffers: new Map(),
            uniforms: new Map()
        });
        
        return contextId;
    },
    
    createShader: function(contextId, type, source) {
        const ctx = this.contexts.get(contextId);
        if (!ctx) return null;
        
        const gl = ctx.gl;
        const shader = gl.createShader(type === 35633 ? gl.VERTEX_SHADER : gl.FRAGMENT_SHADER);
        gl.shaderSource(shader, source);
        gl.compileShader(shader);
        
        if (!gl.getShaderParameter(shader, gl.COMPILE_STATUS)) {
            const error = gl.getShaderInfoLog(shader);
            console.error('Shader compilation error:', error);
            gl.deleteShader(shader);
            return null;
        }
        
        const shaderId = `shader_${Date.now()}_${Math.random()}`;
        ctx.shaders.set(shaderId, shader);
        return shaderId;
    },
    
    createProgram: function(contextId, vertexShaderId, fragmentShaderId) {
        const ctx = this.contexts.get(contextId);
        if (!ctx) return null;
        
        const gl = ctx.gl;
        const program = gl.createProgram();
        
        if (vertexShaderId) {
            const vs = ctx.shaders.get(vertexShaderId);
            if (vs) gl.attachShader(program, vs);
        }
        
        if (fragmentShaderId) {
            const fs = ctx.shaders.get(fragmentShaderId);
            if (fs) gl.attachShader(program, fs);
        }
        
        gl.linkProgram(program);
        
        if (!gl.getProgramParameter(program, gl.LINK_STATUS)) {
            const error = gl.getProgramInfoLog(program);
            console.error('Program linking error:', error);
            gl.deleteProgram(program);
            return null;
        }
        
        const programId = `program_${Date.now()}_${Math.random()}`;
        ctx.programs.set(programId, program);
        return programId;
    },
    
    useProgram: function(contextId, programId) {
        const ctx = this.contexts.get(contextId);
        if (!ctx || !programId) return;
        
        const gl = ctx.gl;
        const program = ctx.programs.get(programId);
        if (program) {
            gl.useProgram(program);
        }
    },
    
    setUniform3f: function(contextId, programId, name, x, y, z) {
        const ctx = this.contexts.get(contextId);
        if (!ctx) return;
        
        const gl = ctx.gl;
        const program = ctx.programs.get(programId);
        if (!program) return;
        
        gl.useProgram(program);
        const location = gl.getUniformLocation(program, name);
        if (location) {
            gl.uniform3f(location, x, y, z);
        }
    },
    
    setUniform1fv: function(contextId, programId, name, values) {
        const ctx = this.contexts.get(contextId);
        if (!ctx) return;
        
        const gl = ctx.gl;
        const program = ctx.programs.get(programId);
        if (!program) return;
        
        gl.useProgram(program);
        const location = gl.getUniformLocation(program, name);
        if (location) {
            gl.uniform1fv(location, new Float32Array(values));
        }
    },
    
    createFramebuffer: function(contextId, width, height) {
        const ctx = this.contexts.get(contextId);
        if (!ctx) return null;
        
        const gl = ctx.gl;
        const fbo = gl.createFramebuffer();
        gl.bindFramebuffer(gl.FRAMEBUFFER, fbo);
        
        // Create color texture
        const texture = gl.createTexture();
        gl.bindTexture(gl.TEXTURE_2D, texture);
        gl.texImage2D(gl.TEXTURE_2D, 0, gl.RGBA16F, width, height, 0, gl.RGBA, gl.HALF_FLOAT, null);
        gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MIN_FILTER, gl.LINEAR);
        gl.texParameteri(gl.TEXTURE_2D, gl.TEXTURE_MAG_FILTER, gl.LINEAR);
        
        gl.framebufferTexture2D(gl.FRAMEBUFFER, gl.COLOR_ATTACHMENT0, gl.TEXTURE_2D, texture, 0);
        
        // Create depth buffer
        const depthBuffer = gl.createRenderbuffer();
        gl.bindRenderbuffer(gl.RENDERBUFFER, depthBuffer);
        gl.renderbufferStorage(gl.RENDERBUFFER, gl.DEPTH_COMPONENT16, width, height);
        gl.framebufferRenderbuffer(gl.FRAMEBUFFER, gl.DEPTH_ATTACHMENT, gl.RENDERBUFFER, depthBuffer);
        
        gl.bindFramebuffer(gl.FRAMEBUFFER, null);
        
        const fboId = `fbo_${Date.now()}_${Math.random()}`;
        ctx.framebuffers.set(fboId, { fbo, texture, depthBuffer, width, height });
        return fboId;
    },
    
    bindFramebuffer: function(contextId, fboId) {
        const ctx = this.contexts.get(contextId);
        if (!ctx) return;
        
        const gl = ctx.gl;
        if (fboId) {
            const fboData = ctx.framebuffers.get(fboId);
            if (fboData) {
                gl.bindFramebuffer(gl.FRAMEBUFFER, fboData.fbo);
            }
        } else {
            gl.bindFramebuffer(gl.FRAMEBUFFER, null);
        }
    },
    
    setViewport: function(contextId, x, y, width, height) {
        const ctx = this.contexts.get(contextId);
        if (!ctx) return;
        
        ctx.gl.viewport(x, y, width, height);
    },
    
    clear: function(contextId, r, g, b, a) {
        const ctx = this.contexts.get(contextId);
        if (!ctx) return;
        
        const gl = ctx.gl;
        gl.clearColor(r, g, b, a);
        gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);
    },
    
    renderQuad: function(contextId) {
        const ctx = this.contexts.get(contextId);
        if (!ctx) return;
        
        const gl = ctx.gl;
        
        // Create quad if it doesn't exist
        if (!ctx.quadBuffer) {
            const positions = new Float32Array([
                -1, -1,
                 1, -1,
                -1,  1,
                 1,  1
            ]);
            
            ctx.quadBuffer = gl.createBuffer();
            gl.bindBuffer(gl.ARRAY_BUFFER, ctx.quadBuffer);
            gl.bufferData(gl.ARRAY_BUFFER, positions, gl.STATIC_DRAW);
        }
        
        // Find the position attribute location
        const positionLocation = gl.getAttribLocation(gl.getParameter(gl.CURRENT_PROGRAM), 'a_position');
        if (positionLocation >= 0) {
            gl.bindBuffer(gl.ARRAY_BUFFER, ctx.quadBuffer);
            gl.enableVertexAttribArray(positionLocation);
            gl.vertexAttribPointer(positionLocation, 2, gl.FLOAT, false, 0, 0);
            gl.drawArrays(gl.TRIANGLE_STRIP, 0, 4);
        }
    },
    
    resize: function(contextId, width, height) {
        const ctx = this.contexts.get(contextId);
        if (!ctx) return;
        
        ctx.canvas.width = width;
        ctx.canvas.height = height;
        ctx.gl.viewport(0, 0, width, height);
    },
    
    captureScreenshot: function(contextId, format) {
        const ctx = this.contexts.get(contextId);
        if (!ctx) return null;
        
        const gl = ctx.gl;
        const canvas = ctx.canvas;
        const width = canvas.width;
        const height = canvas.height;
        
        // Read pixels from WebGL context
        const pixels = new Uint8Array(width * height * 4);
        gl.readPixels(0, 0, width, height, gl.RGBA, gl.UNSIGNED_BYTE, pixels);
        
        // Flip vertically (WebGL has origin at bottom-left, canvas at top-left)
        const flipped = new Uint8Array(width * height * 4);
        for (let y = 0; y < height; y++) {
            const srcRow = (height - 1 - y) * width * 4;
            const dstRow = y * width * 4;
            flipped.set(pixels.subarray(srcRow, srcRow + width * 4), dstRow);
        }
        
        // Convert to ImageData
        const imageData = new ImageData(
            new Uint8ClampedArray(flipped),
            width,
            height
        );
        
        // Create temporary canvas to convert to blob
        const tempCanvas = document.createElement('canvas');
        tempCanvas.width = width;
        tempCanvas.height = height;
        const tempCtx = tempCanvas.getContext('2d');
        tempCtx.putImageData(imageData, 0, 0);
        
        return new Promise((resolve) => {
            tempCanvas.toBlob((blob) => {
                if (!blob) {
                    resolve(null);
                    return;
                }
                
                // Convert blob to base64 for C# interop
                const reader = new FileReader();
                reader.onloadend = () => {
                    const base64 = reader.result.split(',')[1];
                    // Convert base64 to byte array
                    const binaryString = atob(base64);
                    const bytes = new Uint8Array(binaryString.length);
                    for (let i = 0; i < binaryString.length; i++) {
                        bytes[i] = binaryString.charCodeAt(i);
                    }
                    resolve(Array.from(bytes));
                };
                reader.readAsDataURL(blob);
            }, format === 'jpg' ? 'image/jpeg' : 'image/png', format === 'jpg' ? 0.8 : undefined);
        });
    },
    
    cleanup: function(contextId) {
        const ctx = this.contexts.get(contextId);
        if (!ctx) return;
        
        const gl = ctx.gl;
        
        // Clean up shaders
        ctx.shaders.forEach(shader => gl.deleteShader(shader));
        ctx.shaders.clear();
        
        // Clean up programs
        ctx.programs.forEach(program => gl.deleteProgram(program));
        ctx.programs.clear();
        
        // Clean up framebuffers
        ctx.framebuffers.forEach(fboData => {
            gl.deleteFramebuffer(fboData.fbo);
            gl.deleteTexture(fboData.texture);
            gl.deleteRenderbuffer(fboData.depthBuffer);
        });
        ctx.framebuffers.clear();
        
        this.contexts.delete(contextId);
    }
};

