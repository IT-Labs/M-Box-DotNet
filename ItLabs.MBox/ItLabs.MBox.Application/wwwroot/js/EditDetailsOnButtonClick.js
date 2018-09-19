$(document).ready(function () {
    $('.saveDetails').hide();
    $('.canselDetails').hide();
});

function editDetails(elementId) {
    document.getElementById(elementId).disabled = false;
}

function saveDetails(elementId) {
    document.getElementById(elementId).disabled = true;
    savedValue = document.getElementById(elementId).value;
}

function cancelEditiong(elementId) { 
    document.getElementById(elementId).value = savedValue;
    document.getElementById(elementId).disabled = true;
}