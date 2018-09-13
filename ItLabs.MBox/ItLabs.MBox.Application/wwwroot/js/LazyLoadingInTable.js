var modelJSValue = {};

jQuery(document).ready(function () {
    modelJSValue.RecordLabels = $("#recordlabels").val();
    modelJSValue.Take = $("#take").val();
    modelJSValue.Skip = $("#skip").val();
});

$('tbody').on("scroll", function () {
    var scrollHeight = 0;
    $('tr').each(function () {
        scrollHeight += $(this).height();
    });
    var scrollPosition = $('tbody').height() + $('tbody').scrollTop();
    if ((scrollHeight - scrollPosition) / scrollHeight === 0) {

        modelJSValue.Skip = parseInt(modelJSValue.Skip) + parseInt(modelJSValue.Take);
        modelJSValue.Take = 10;
        var toSend = jQuery.param(modelJSValue);

        $.ajax({
            type: "GET",
            contentType: "application/json",
            url: "/Admin/GetNextRecordLabels",
            data: toSend,
            success: function (result) {
                $("#recordLabelsListId").append(result);
            }
        })
    }
});