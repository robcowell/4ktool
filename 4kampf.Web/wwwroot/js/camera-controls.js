// Camera Controls for WebGL
window.cameraControls = {
    cameras: new Map(),
    
    init: function(cameraId) {
        this.cameras.set(cameraId, {
            position: { x: 0, y: 0, z: 0 },
            rotation: { x: 0, y: Math.PI, z: 0 },
            forward: { x: 0, y: 0, z: 0 },
            right: { x: 0, y: 0, z: 0 },
            up: { x: 0, y: 0, z: 0 },
            editStep: 0.1,
            mode: 'freefly', // 'freefly' or 'lockfly'
            keys: {},
            mouseDown: false,
            mouseStartPos: { x: 0, y: 0 },
            mouseCurrentPos: { x: 0, y: 0 }
        });
        
        return cameraId;
    },
    
    reset: function(cameraId) {
        const cam = this.cameras.get(cameraId);
        if (!cam) return;
        
        cam.position = { x: 0, y: 0, z: 0 };
        cam.rotation = { x: 0, y: Math.PI, z: 0 };
    },
    
    updateVectors: function(cameraId) {
        const cam = this.cameras.get(cameraId);
        if (!cam) return;
        
        // Clamp pitch
        if (cam.rotation.x < -Math.PI / 2.0) cam.rotation.x = -Math.PI / 2.0;
        if (cam.rotation.x > Math.PI / 2.0) cam.rotation.x = Math.PI / 2.0;
        
        // Calculate forward, right, up vectors from rotation
        const cosX = Math.cos(cam.rotation.x);
        const sinX = Math.sin(cam.rotation.x);
        const cosY = Math.cos(cam.rotation.y);
        const sinY = Math.sin(cam.rotation.y);
        
        cam.forward = {
            x: sinY * cosX,
            y: -sinX,
            z: -cosY * cosX
        };
        
        cam.right = {
            x: cosY,
            y: 0,
            z: sinY
        };
        
        cam.up = {
            x: -sinY * sinX,
            y: cosX,
            z: cosY * sinX
        };
    },
    
    move: function(cameraId, amount) {
        const cam = this.cameras.get(cameraId);
        if (!cam) return;
        
        cam.position.x -= cam.forward.x * amount;
        cam.position.y -= cam.forward.y * amount;
        cam.position.z -= cam.forward.z * amount;
    },
    
    strafe: function(cameraId, amount) {
        const cam = this.cameras.get(cameraId);
        if (!cam) return;
        
        cam.position.x -= cam.right.x * amount;
        cam.position.y -= cam.right.y * amount;
        cam.position.z -= cam.right.z * amount;
    },
    
    crane: function(cameraId, amount) {
        const cam = this.cameras.get(cameraId);
        if (!cam) return;
        
        cam.position.y += amount;
    },
    
    mouselook: function(cameraId, deltaX, deltaY) {
        const cam = this.cameras.get(cameraId);
        if (!cam) return;
        
        const PI_OVER_180 = 0.0174532925;
        cam.rotation.y = (cam.rotation.y - deltaX * PI_OVER_180) % (2 * Math.PI);
        cam.rotation.x = (cam.rotation.x - deltaY * PI_OVER_180) % (2 * Math.PI);
    },
    
    getPosition: function(cameraId) {
        const cam = this.cameras.get(cameraId);
        if (!cam) return { x: 0, y: 0, z: 0 };
        return { ...cam.position };
    },
    
    getRotation: function(cameraId) {
        const cam = this.cameras.get(cameraId);
        if (!cam) return { x: 0, y: Math.PI, z: 0 };
        return { ...cam.rotation };
    },
    
    setKeyState: function(cameraId, key, pressed) {
        const cam = this.cameras.get(cameraId);
        if (!cam) return;
        cam.keys[key] = pressed;
    },
    
    setMouseState: function(cameraId, down, x, y) {
        const cam = this.cameras.get(cameraId);
        if (!cam) return;
        
        cam.mouseDown = down;
        if (down) {
            cam.mouseStartPos = { x, y };
        }
        cam.mouseCurrentPos = { x, y };
    },
    
    setMode: function(cameraId, mode) {
        const cam = this.cameras.get(cameraId);
        if (!cam) return;
        cam.mode = mode; // 'freefly' or 'lockfly'
    },
    
    getMode: function(cameraId) {
        const cam = this.cameras.get(cameraId);
        return cam ? cam.mode : 'freefly';
    },
    
    update: function(cameraId, shiftPressed) {
        const cam = this.cameras.get(cameraId);
        if (!cam) return;
        
        const MOVE_SPEED = 4.0;
        const MOUSE_LOOK_SPEED = 0.5;
        const speedMove = MOVE_SPEED * (shiftPressed ? 0.02 : 1.0);
        const speedLook = MOUSE_LOOK_SPEED * (shiftPressed ? 0.02 : 1.0);
        
        // Handle keyboard movement
        if (cam.keys['w'] || cam.keys['W'] || cam.keys['ArrowUp']) {
            this.move(cameraId, -speedMove * cam.editStep);
        }
        if (cam.keys['s'] || cam.keys['S'] || cam.keys['ArrowDown']) {
            this.move(cameraId, speedMove * cam.editStep);
        }
        if (cam.keys['a'] || cam.keys['A'] || cam.keys['ArrowLeft']) {
            this.strafe(cameraId, -speedMove * cam.editStep);
        }
        if (cam.keys['d'] || cam.keys['D'] || cam.keys['ArrowRight']) {
            this.strafe(cameraId, speedMove * cam.editStep);
        }
        if (cam.keys['PageUp'] || cam.keys['r'] || cam.keys['R']) {
            this.crane(cameraId, speedMove * cam.editStep);
        }
        if (cam.keys['PageDown'] || cam.keys['f'] || cam.keys['F']) {
            this.crane(cameraId, -speedMove * cam.editStep);
        }
        
        // Handle mouse look
        if (cam.mouseDown) {
            const deltaX = (cam.mouseCurrentPos.x - cam.mouseStartPos.x) * speedLook;
            const deltaY = (cam.mouseCurrentPos.y - cam.mouseStartPos.y) * speedLook;
            
            // Lockfly mode: lock Y rotation (horizontal only)
            if (cam.mode === 'lockfly') {
                this.mouselook(cameraId, deltaX, 0);
            } else {
                // Freefly mode: full rotation
                this.mouselook(cameraId, deltaX, deltaY);
            }
            
            cam.mouseStartPos = { ...cam.mouseCurrentPos };
        }
        
        this.updateVectors(cameraId);
    }
};

