var modelJSValue = {};
var lazyLoadingUrl = $("#lazyLoadingUrl").val();

jQuery(document).ready(function () {
    modelJSValue.Take = $("#take").val();
    modelJSValue.Skip = $("#skip").val();
    modelJSValue.Id = $("#artistId").val();
});

$(window).on("scroll", function () {
    var scrollHeight = $(document).height();
    var scrollPosition = $(window).height() + $(window).scrollTop();
    if ((scrollHeight - scrollPosition) / scrollHeight === 0) {
        modelJSValue.Skip = parseInt(modelJSValue.Skip) + parseInt(modelJSValue.Take);
        modelJSValue.Take = 10;
        var toSend = jQuery.param(modelJSValue);
        console.log(toSend);
            $.ajax({
                type: "GET",
                contentType: "application/json",
                url: lazyLoadingUrl,
                data: toSend,
                success: function (result) {
                    $("#songListId").append(result);
                }
            });
         
    }
});
