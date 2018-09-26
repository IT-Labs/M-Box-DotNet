var modelJSValue = {
    PagingModelSongs : {},
    Artist : { Id: 0}
};
var pagingModel = {};
var lazyLoadingUrl = $("#lazyLoadingUrl").val();

jQuery(document).ready(function () {
    pagingModel.PagingList = $("#songList").val();
    pagingModel.Take = $("#take").val();
    pagingModel.Skip = $("#skip").val();
    modelJSValue.PagingModelSongs = pagingModel;
    modelJSValue.Artist.Id = $("#artistId").val();
});

$(window).on("scroll", function () {
    var scrollHeight = $(document).height();
    var scrollPosition = $(window).height() + $(window).scrollTop();
    if ((scrollHeight - scrollPosition) / scrollHeight === 0) {
        modelJSValue.PagingModelSongs.Skip = parseInt(modelJSValue.PagingModelSongs.Skip) + parseInt(modelJSValue.PagingModelSongs.Take);
        modelJSValue.PagingModelSongs.Take = 10;
        var toSend = jQuery.param(modelJSValue);
            $.ajax({
                type: "POST",
                contentType: "application/json",
                url: lazyLoadingUrl,
                data: toSend,
                success: function (result) {
                    $("#songListId").append(result);
                }
            });
         
    }
});
