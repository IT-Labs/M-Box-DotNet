var modelJSValue = {};
var lazyLoadingUrl = $("#lazyLoadingUrl").val();

$(document).ready(function () {
    modelJSValue.RecordLabels = $("#recordlabels").val();
    modelJSValue.Take = $("#take").val();
    modelJSValue.Skip = $("#skip").val();
});

$(window).on("scroll", function () {
    var scrollHeight = $(document).height();
    var scrollPosition = $(window).height() + $(window).scrollTop();
    
    if ((scrollHeight - scrollPosition) / scrollHeight === 0) {
        modelJSValue.Skip = parseInt(modelJSValue.Skip) + parseInt(modelJSValue.Take);
        modelJSValue.Take = 10;
        var toSend = $.param(modelJSValue);

        $.ajax({
            type: "GET",
            contentType: "application/json",
            url: lazyLoadingUrl,
            data: toSend,
            success: function (result) {
                $("#recordLabelsList").append(result);
            }
        });
    }
});