$(document).ready(function () {
    function Contains(text1, text2) {
        if (text1.indexOf(text2) != -1)
            return true
    }
    $("#search").keyup(function () {
        var searchText = $("#search").val().toLocaleLowerCase();
        $(".search").each(function () {
            if (!Contains($(this).text().toLocaleLowerCase(), searchText)) {
                $(this).hide();
            } else {
                $(this).show();
            }
        });
    });
});