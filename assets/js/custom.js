function transType() {
    var btnid = document.getElementById("isDep");
    if (btnid.getAttribute("value") == "true") {
        btnid.setAttribute("value", "false");
        btnid.innerHTML = "Withdraw";
        btnid.className = "mdc-button mdc-button--unelevated filled-button--danger mdc-ripple-upgraded";
    } else {
        btnid.setAttribute("value", "true");
        btnid.innerHTML = "Deposit";
        btnid.className = "mdc-button mdc-button--unelevated filled-button--success mdc-ripple-upgraded";
    };
};

function uploadFile() {
    uploader.click();
};

function formSub() {
    document.getElementById("transaction").submit();
}

function loadFile(event) {
    formSub();
};
