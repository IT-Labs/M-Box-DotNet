$(document).ready(function () {
    $('.saveDetails').hide();
    $('.cancelDetails').hide();
});

function editDetails(elementId, edit, save, cancel) {
    document.getElementById(elementId).disabled = false;
    savedValue = document.getElementById(elementId).value;

    $('#' + save).show();
    $('#' + cancel).show();
    $('#' + edit).hide();
}

function saveDetails(elementId, edit, save, cancel) {
    document.getElementById(elementId).disabled = true;   

    $('#' + save).hide();
    $('#' + cancel).hide();
    $('#' + edit).show();
}

function cancelEditiong(elementId, edit, save, cancel) { 
    document.getElementById(elementId).value = savedValue;
    document.getElementById(elementId).disabled = true;

    $('#' + save).hide();
    $('#' + cancel).hide();
    $('#' + edit).show();
}