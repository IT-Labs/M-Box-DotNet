
var modelJSValue = {};
var lazyLoadingUrl = $("#lazyLoadingUrl").val();

$(document).ready(function () {
    modelJSValue.Take = $("#take").val();
    modelJSValue.Skip = $("#skip").val();
    modelJSValue.Following = $("#following").val();
});

$(window).on("scroll", function () {
    var scrollHeight = $(document).height();
    var scrollPosition = $(window).height() + $(window).scrollTop();
    if ((scrollHeight - scrollPosition) / scrollHeight === 0) {
        modelJSValue.Skip = parseInt(modelJSValue.Skip) + parseInt(modelJSValue.Take);
        modelJSValue.Take = 10;
        modelJSValue.SearchQuery = $("#searchBox").val();
        var toSend = jQuery.param(modelJSValue);
        $.ajax({
            type: "GET",
            contentType: "application/json",
            url: lazyLoadingUrl,
            data: toSend,
            success: function (result) {
                $("#listToAppend").append(result);
            }
        });

    }
});
$("#listToAppend").on("click", ".clickableButton", function () {
    var artistId = this.getAttribute("data-artistId");
    var toSend = { artistId: parseInt(artistId) };
    if (!window.confirm("Are you sure?")) {
        return;
    }

    $.ajax({
        type: "GET",
        contentType: "application/json",
        url: "/Home/ToggleFollow",
        data: $.param(toSend),
        success: function (result) {
            $("button[data-artistId='" + artistId + "']").parent().parent().remove();
        }
    });
});

