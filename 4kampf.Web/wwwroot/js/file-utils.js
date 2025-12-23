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
 * Creates and downloads a zip file containing multiple files
 * Requires JSZip library to be loaded
 * @param {string} zipFilename - Name of the zip file to download
 * @param {Array<{filename: string, content: string}>} files - Array of file objects with filename and content
 * @returns {Promise} Promise that resolves when zip is downloaded
 */
window.downloadZip = async function(zipFilename, files) {
    // Check if JSZip is available
    if (typeof JSZip === 'undefined') {
        // Load JSZip from CDN if not available
        await new Promise((resolve, reject) => {
            const script = document.createElement('script');
            script.src = 'https://cdnjs.cloudflare.com/ajax/libs/jszip/3.10.1/jszip.min.js';
            script.onload = resolve;
            script.onerror = reject;
            document.head.appendChild(script);
        });
    }
    
    const zip = new JSZip();
    
    // Add all files to zip
    files.forEach(file => {
        zip.file(file.filename, file.content);
    });
    
    // Generate zip and download
    const blob = await zip.generateAsync({ type: 'blob' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = zipFilename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(url);
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

