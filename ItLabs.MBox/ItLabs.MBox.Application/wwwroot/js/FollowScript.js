$("#followButton").on("click", function () {
    var toSend = { artistId : parseInt($("#artistId").val()) };
    $.ajax({
        type: "GET",
        contentType: "application/json",
        url: "/Home/ToggleFollow",
        data: $.param(toSend),
        success: function (result) {
            if ($("#followButton").html() == "Follow") {
                $("#followButton").html("Unfollow");
            } else {
                $("#followButton").html("Follow");
            }
        }, error: function () {
            window.location = "/Account/Login"
        }
    });
});