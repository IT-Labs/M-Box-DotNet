
$(".forsearch").on("input propertychange paste", function () {
    $("#search").attr("action", "/Home/MainSearch?searchValue=" + $(".forsearch").val());
});
