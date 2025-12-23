// File utility functions for 4kampf Web

/**
 * Downloads a file with the given content
 * @param {string} filename - Name of the file to download
 * @param {string} content - File content (can be data URL or plain text)
 * @param {string} mimeType - MIME type (e.g., 'text/plain', 'application/xml')
 */
window.downloadFile = function(filename, content, mimeType) {
    let blob;
    if (content.startsWith('data:')) {
        // Content is already a data URL
        const response = fetch(content).then(res => res.blob()).then(blob => {
            const url = URL.createObjectURL(blob);
            const link = document.createElement('a');
            link.href = url;
            link.download = filename;
            document.body.appendChild(link);
            link.click();
            document.body.removeChild(link);
            URL.revokeObjectURL(url);
        });
    } else {
        blob = new Blob([content], { type: mimeType || 'text/plain' });
        const url = URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = filename;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(url);
    }
};

/**
 * Toggles fullscreen for an element
 */
window.toggleFullscreen = function(elementId) {
    const element = document.getElementById(elementId);
    if (!element) return;
    
    if (!document.fullscreenElement) {
        element.requestFullscreen().catch(err => {
            console.error('Error attempting to enable fullscreen:', err);
        });
    } else {
        document.exitFullscreen();
    }
};

