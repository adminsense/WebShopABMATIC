window.adminExport = {
    downloadCsv: function (fileName, base64Content) {
        const link = document.createElement('a');
        link.href = 'data:text/csv;charset=utf-8;base64,' + base64Content;
        link.download = fileName;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
    },
    printPdf: function (title, htmlContent) {
        const printWindow = window.open('', '_blank', 'width=1024,height=768');
        if (!printWindow) {
            alert('Pop-up blocked. Allow pop-ups to export PDF.');
            return;
        }
        printWindow.document.open();
        printWindow.document.write(htmlContent);
        printWindow.document.close();
        printWindow.focus();
        printWindow.onload = function () {
            printWindow.print();
        };
    }
};
