// JS for Board

$(function () {

    $("#dialog-message").dialog({
        modal: false,
        autoOpen: false,
        closeOnEscape: true,
        position:
        {
            my: 'left+7 top',
            at: 'left bottom+5',
            of: $('.board.new')
        }
    }).show();


    $(document).on('click', '.userBoardLink', function (event) {
        event.stopPropagation();
        $(this).closest('form').submit();
    });


    $(document).on('click', '.board:not(.new)', function () { // click on existing board => redirect to tickets 
        var id = $(this).attr('id');
        var domain = window.location.host;
        window.location.replace("http://" + domain + "/Boards/Details/" + id);
    });



    $(document).on('click', '.board.new', function () { // open new board dialog
        if ($('#dialog-message').dialog("isOpen")) $('#dialog-message').dialog("close");
        else $('#dialog-message').dialog("open");
    });


    $(document).on('click', '.close_dialog', function () { // close dialog button
        $(this).closest("#dialog-message").dialog("close");
    });


    $(window).click(function (event) { // handle click outside of dialog :) it works :)
        if ($(event.target.closest('.ui-dialog')).length !== 1 && $(event.target.closest('.board.new')).length !== 1) {
            $("#dialog-message").dialog("close");
        }
    });

    $(document).on('click', '.delete-board', function (event) {
        event.stopPropagation();
        var boardId = $(this).closest('.board').attr('id');
        var verificationToken = $('#__AjaxAntiForgeryForm input').val();

        if (boardId != null) {
            var data = {
                __RequestVerificationToken: verificationToken,
                boardId: boardId
            };

            $.ajax({
                url: "/Boards/Delete",
                type: "POST",
                data: data,
                success: function (data) {
                    var domain = window.location.host;
                    window.location.replace("http://" + domain + "/Boards/Index");
                }

            });

        } else {
            alert("There was some error in deleting this ticket. Try reload the page and do this action again.");
        }
    });

});








 