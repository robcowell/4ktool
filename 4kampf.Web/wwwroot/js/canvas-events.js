// Canvas Event Handlers for Camera Controls
window.canvasEvents = {
    init: function(canvasId, cameraId) {
        const canvas = document.getElementById(canvasId);
        if (!canvas) return;
        
        // Store last key event for shift key detection
        window.lastKeyEvent = null;
        
        // Keyboard events
        canvas.addEventListener('keydown', (e) => {
            window.lastKeyEvent = e;
            const key = e.key;
            window.cameraControls.setKeyState(cameraId, key, true);
            e.preventDefault();
        });
        
        canvas.addEventListener('keyup', (e) => {
            window.lastKeyEvent = e;
            const key = e.key;
            window.cameraControls.setKeyState(cameraId, key, false);
            e.preventDefault();
        });
        
        // Mouse events
        let mouseDown = false;
        let lastMouseX = 0;
        let lastMouseY = 0;
        
        canvas.addEventListener('mousedown', (e) => {
            if (e.button === 2) { // Right mouse button
                mouseDown = true;
                lastMouseX = e.clientX;
                lastMouseY = e.clientY;
                window.cameraControls.setMouseState(cameraId, true, e.clientX, e.clientY);
                canvas.requestPointerLock();
                e.preventDefault();
            }
        });
        
        canvas.addEventListener('mouseup', (e) => {
            if (e.button === 2) {
                mouseDown = false;
                window.cameraControls.setMouseState(cameraId, false, e.clientX, e.clientY);
                document.exitPointerLock();
                e.preventDefault();
            }
        });
        
        canvas.addEventListener('mousemove', (e) => {
            if (mouseDown && document.pointerLockElement === canvas) {
                const deltaX = e.movementX || 0;
                const deltaY = e.movementY || 0;
                
                if (deltaX !== 0 || deltaY !== 0) {
                    window.cameraControls.mouselook(cameraId, deltaX, deltaY);
                }
            } else {
                window.cameraControls.setMouseState(cameraId, mouseDown, e.clientX, e.clientY);
            }
        });
        
        // Prevent context menu on right click
        canvas.addEventListener('contextmenu', (e) => {
            e.preventDefault();
        });
        
        // Handle pointer lock changes
        document.addEventListener('pointerlockchange', () => {
            if (document.pointerLockElement !== canvas) {
                mouseDown = false;
                window.cameraControls.setMouseState(cameraId, false, 0, 0);
            }
        });
    }
};

