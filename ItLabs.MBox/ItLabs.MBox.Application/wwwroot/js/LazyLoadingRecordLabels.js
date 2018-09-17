var modelJSValue = {};
var loadCounter;
var loadsBeforeLoadMoreButtonAppears = 10;
var lazyLoadingUrl = $("#lazyLoadingUrl").val();

jQuery(document).ready(function () {
    modelJSValue.RecordLabels = $("#recordlabels").val();
    modelJSValue.Take = $("#take").val();
    modelJSValue.Skip = $("#skip").val();
    $("#loadmorebutton").hide();
    loadCounter = 0;
});

$(window).on("scroll", function () {
    var scrollHeight = $(document).height();
    var scrollPosition = $(window).height() + $(window).scrollTop();
    if ((scrollHeight - scrollPosition) / scrollHeight === 0) {

        modelJSValue.Skip = parseInt(modelJSValue.Skip) + parseInt(modelJSValue.Take);
        modelJSValue.Take = 10;
        var toSend = jQuery.param(modelJSValue);
        if (loadCounter < loadsBeforeLoadMoreButtonAppears) {
            $.ajax({
                type: "GET",
                contentType: "application/json",
                url: lazyLoadingUrl,
                data: toSend,
                success: function (result) {
                    $("#recordLabelsListId").append(result);
                    loadCounter++;
                }
            });
        } else {
            $("#loadmorebutton").show();
        }
    }
});

$("#loadmorebutton").on("click", function () {
    var toSend = jQuery.param(modelJSValue);
    loadCounter = 0;
    $("#loadmorebutton").hide();
    $.ajax({
        type: "GET",
        contentType: "application/json",
        data: toSend,
        url: lazyLoadingUrl,
        success: function (result) {
            $("#recordLabelsListId").append(result);
            counter++;
        }
    });
});