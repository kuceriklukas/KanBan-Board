$(document).ready(function() {
    $(document).on('click', '.delete-ticket', function () {
        var ticketId = $(this).closest('.ui-state-default').find('.ticket').attr('id');
        var boardId = $(document).find('#BoardId').val();
        var token = $('#__AjaxAntiForgeryForm input').val();
        //alert(ticketId + " " + boardId);

        if (ticketId != null) {
            var data = {
                __RequestVerificationToken: token,
                ticketId: ticketId
            };

            $.ajax({
                url: "/Tickets/Delete",
                type: "POST",
                data: data,
                success: function (data) {
                    var domain = window.location.host;
                    window.location.replace("http://" + domain + "/Boards/Details/" + boardId);
                }

            });

        } else {
            alert("There was some error in deleting this ticket. Try reload the page and do this action again.");
        }
    });

    $(document).on('click', '.addTicket', function () { // click on add new card with class new - creates ticket first
        
        var boardId = $(document).find('#BoardId').val();
        var token = $('#__AjaxAntiForgeryForm input').val();
        var name = $(this).closest('.ticket').find('.name').text().replace(/\s+/g, ' ').trim();

        if (name == null || name === "New ticket") {
            alert("Crazy? Name can not be New ticket or null");
            return;
        }


        var data = {
            __RequestVerificationToken: token,
            Name: name,
            BoardId: boardId
        };

        $.ajax({
            url: "/Tickets/Create",
            data: data,
            type: "POST",
            success: function(data) {
                var domain = window.location.host;
                window.location.replace("http://" + domain + "/Boards/Details/" + boardId);
            }
        });

    });
});