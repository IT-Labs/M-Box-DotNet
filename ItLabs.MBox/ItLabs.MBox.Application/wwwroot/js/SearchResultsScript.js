$(document).ready(function () {
    $('input:radio[value=' + $('#selectedButton').val() + ']').prop('checked', true);
});
$('input[type=radio]').on('change', function () {
    $(this).closest("form").submit();
});