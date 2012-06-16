var clientGuid
var timeout_ = 15 * 60 * 1000;

var AsyncServiceAddress = "UpdateAsyncHandler.ashx";

$(document).ready(function () {
     Connect();
});

$(window).unload(function () {
    Disconnect();
});

function SendRequest() {
    var url = './' + AsyncServiceAddress + '?guid=' + clientGuid;
    $.ajax({
        type: "POST",
        url: url,
        timeout: timeout_,
        success: ProcessResponse,
        error: SendRequest
    });
}

function Connect() {
    var url = './' + AsyncServiceAddress + '?cmd=register';
    $.ajax({
        type: "POST",
        url: url,
        success: OnConnected,
        error: ConnectionRefused
    });
}

function Disconnect() {
    var url = './' + AsyncServiceAddress + '?cmd=unregister';
    $.ajax({
        type: "POST",
        url: url
    });
}

function ProcessResponse(response_) {
    var response = JSON.parse(response_);
    if (response.isError == false && response.needToUpdate == true) {
        UpdPanelUpdate();
        SendRequest();
    }
    else if (response.isError == true) {
        alert(response.errorMessage);
    }
}

function OnConnected(guid) {
    clientGuid = guid;
    SendRequest();
}

function ConnectionRefused() {
    Connect();
}
