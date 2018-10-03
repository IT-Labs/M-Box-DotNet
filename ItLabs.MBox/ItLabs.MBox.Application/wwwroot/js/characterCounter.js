$(function () {
    $('.characterCounter').keyup(function (e) {
        var max = $('.characterCounter').attr('maxlength');
        var len = $(this).val().length;
        var char = max - len;
        $('#counter').html(char);
    });
});