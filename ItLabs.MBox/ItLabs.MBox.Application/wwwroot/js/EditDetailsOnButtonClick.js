window.onload = function () {
    document.getElementsByClassName('save').style.display = 'none';
    document.getElementsByClassName('cansel').style.display = 'none';
    var songName = document.getElementById('songName').value;
}

function editDetails() {
    document.getElementById('songName').disabled = false;
    document.getElementById('saveSongName').style.display = 'block';
    document.getElementById('cancelSongName').style.display = 'block';
    document.getElementById('editSongName').style.display = 'none';

}

function saveDetails() {
    document.getElementById('songName').disabled = true;
    songName = document.getElementById('songName').value;
    document.getElementById('editSongName').style.display = 'block';
    document.getElementById('saveSongName').style.display = 'none';
    document.getElementById('cancelSongName').style.display = 'none';
}

function cancelEditiong() {
    document.getElementById('songName').value = songName;
    document.getElementById('songName').disabled = true;
    document.getElementById('editSongName').style.display = 'block';
    document.getElementById('saveSongName').style.display = 'none';
    document.getElementById('cancelSongName').style.display = 'none';
}