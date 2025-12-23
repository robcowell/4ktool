// File utility functions for 4kampf Web

/**
 * Downloads a file with the given content
 * @param {string} filename - Name of the file to download
 * @param {string} content - File content
 * @param {string} mimeType - MIME type (e.g., 'text/plain', 'application/xml')
 */
window.downloadFile = function(filename, content, mimeType) {
    const blob = new Blob([content], { type: mimeType || 'text/plain' });
    const url = URL.createObjectURL(blob);
    const link = document.createElement('a');
    link.href = url;
    link.download = filename;
    document.body.appendChild(link);
    link.click();
    document.body.removeChild(link);
    URL.revokeObjectURL(url);
};

