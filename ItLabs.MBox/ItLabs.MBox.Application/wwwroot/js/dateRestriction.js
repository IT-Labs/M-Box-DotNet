$(function () {
    var currentYear = (new Date).getFullYear();
    var currentMonth = (new Date).getMonth();
    var currentDay = (new Date).getDate();

    $("#releaseDate").datepicker({
        dateFormat: 'dd/mm/yy',
        maxDate: new Date(currentYear, currentMonth, currentDay)
    });
});
