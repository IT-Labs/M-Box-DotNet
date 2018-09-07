$(function () {
    $('#Message').keyup(function (e) {
        var max = 200;
        var len = $(this).val().length;
        var char = max - len;
        $('#counter').html(char);
    });
});