$(document).ready(function () {
    $('.saveDetails').hide();
    $('.cancelDetails').hide();
    $('#youtubeLinkUrl').hide();
    $('#vimeoLinkUrl').hide();
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

function editLink(link, url, edit, save, cancel) {
    savedValue = document.getElementById(url).value;

    $('#' + url).show();
    $('#' + save).show();
    $('#' + cancel).show();
    $('#' + edit).hide();
}

function saveLink(link, url, edit, save, cancel) {
    var saveUrl = document.getElementById(url).value;
    $('#' + link).attr('href', saveUrl);

    $('#' + url).hide();
    $('#' + save).hide();
    $('#' + cancel).hide();
    $('#' + edit).show();
}

function cancelEditiongLink(link, url, edit, save, cancel) {
    document.getElementById(url).value = savedValue;

    $('#' + url).hide();
    $('#' + save).hide();
    $('#' + cancel).hide();
    $('#' + edit).show();
}