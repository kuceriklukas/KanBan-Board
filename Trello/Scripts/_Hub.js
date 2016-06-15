
$(function () {

    // Reference the auto-generated proxy for the hub.
    var userActivity = $.connection.userActivityHub;

    userActivity.client.connected = function (username) { // on client connected
        console.log(username + " onconnected");
        $("#connectedUsers").show();

        var usernameId = username.replace(/[^a-zA-Z 0-9]+/g, '');
        if ($("#connectedUserList").length > 0)
        {
            if ($("#connectedUserList").find("#" + usernameId).length === 0) {
                $("#connectedUserList").append("<div class='col-xs-12 text-center' style='color:#4d4d4d' id=" + usernameId + ">" + username + " <i class='fa fa-circle' style='color:green'></i></div>");
            } else {
                $("#" + usernameId + " i").css({ "color": "green" });
            }
        }

    };


    userActivity.client.disconnect = function (username) { // on client disconnected
        console.log(username + " ondisconnected");


        var usernameId = username.replace(/[^a-zA-Z 0-9]+/g, '');
        if ($("#connectedUserList").length > 0)
        {
            if ($("#connectedUserList").find("#" + usernameId).length > 0) {
                $("#" + usernameId+" i").css({"color":"red"});
            }
        }
    };

    $.connection.hub.start().done(function() {
        userActivity.server.getConnectedUsers().done(function(users) { // get connected users
            $.each(users, function (i, username) {
                console.log(username + " user");

                var usernameId = username.replace(/[^a-zA-Z 0-9]+/g, '');
                $("#connectedUsers").show();
                if ($("#connectedUserList").length > 0) {

                    if ($("#connectedUserList").find("#" + usernameId).length === 0) {
                        $("#connectedUserList").append("<div class='col-xs-12 text-center' style='color:#4d4d4d;' id=" + usernameId + ">" + username + " <i class='fa fa-circle' style='color:green'></i></div><hr style='margin-top:10px; margin-bottom:10px'>");
                    }
                }
            });
        });
    });

});

