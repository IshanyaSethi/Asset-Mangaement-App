window.downloadFile = function (filename, base64Data) {
    try {
        const link = document.createElement('a');
        link.download = filename;
        link.href = 'data:text/csv;base64,' + base64Data;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        console.log('File download triggered:', filename);
    } catch (error) {
        console.error('Download error:', error);
        alert('Failed to download file: ' + error.message);
    }
};

console.log('fileDownload.js loaded successfully');