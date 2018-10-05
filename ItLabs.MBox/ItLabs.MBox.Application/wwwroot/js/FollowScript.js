$("#followButton").on("click", function () {
    var toSend = { artistId: parseInt($("#artistId").val()) };
    if ($("#followButton").html() == "Unfollow") {
        if (!window.confirm("Are you sure?")) {
            return;
        }
    }
    $.ajax({
        type: "GET",
        contentType: "application/json",
        url: "/Home/ToggleFollow",
        data: $.param(toSend),
        success: function (result) {
            if ($("#followButton").html() == "Follow") {
                $("#followButton").html("Unfollow");
                $("#followersCount").html(parseInt($("#followersCount").html()) + 1);
            } else {
                $("#followButton").html("Follow");
                $("#followersCount").html(parseInt($("#followersCount").html()) -1);
            }
        }, error: function () {
            window.location = "/Account/Login"
        }
    });
});