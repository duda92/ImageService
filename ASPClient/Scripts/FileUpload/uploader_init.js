var uploader;

function init_uploader() {
    uploader = new qq.FileUploader({ element: document.getElementById('file_upload'), action: 'UploadManager.ashx',
        onComplete: function (id, fileName, responseJSON) {
            selected_file_name = fileName; UpdPanelUpdate();
        }
    });

}