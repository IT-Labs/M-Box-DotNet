$(function () {
    var date = new Date();

    var currentYear = date.getFullYear();
    var currentMonth = date.getMonth();
    var currentDay = date.getDate();

    $(".dateOfRelease").datepicker({
        dateFormat: 'dd/mm/yy',
        maxDate: new Date(currentYear, currentMonth, currentDay)
    });
});
