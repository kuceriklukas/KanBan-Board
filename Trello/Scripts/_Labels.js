function ColorDialogEvent() {
    var $dialog = $('#label-dialog');
    if ($dialog.hasClass('ui-dialog-content')) {
        $('#label-dialog').dialog('close');
    }
    $('#label-dialog').dialog({
        modal: false,
        autoOpen: false,
        closeOnEscape: true,
        position:
        {
            my: 'left-310 top',
            at: 'left bottom+5',
            of: $('#labelsDialog')
        }

    }).show();
};

$(function () {
    

    $(document).on('click', '#labelsDialog', function () { // opens label dialog with color picker       
        ColorDialogEvent();
        if ($('#label-dialog').dialog("isOpen")) {
            $('#label-dialog').dialog("close");
        }
        else $('#label-dialog').dialog("open");      
    });



    $(document).on('click', '.close_dialog', function () { // close label color dialog
        $(this).closest('#label-dialog').dialog('close');
        $(this).closest('#label-dialog').dialog("destroy");
    });



    $(document).on('click', '.add_card_label', function () { // add card label
        var labelId = $(this).attr('id');
        var strippedLabelId = labelId.substring(3);
        var cardId = $("#CardId").val();
        var verificationToken = $('#__AjaxAntiForgeryFormLabels input').val();
        var data = {
            __RequestVerificationToken: verificationToken,
            CardId: cardId,
            LabelId: strippedLabelId
        };

        //create new label part
        $.ajax({
            url: "/Labels/Create",
            type: "POST",
            data: data,
            success: function (data) {
                $("#label-area").html(data);

                ReloadCards();

                var labelObject = '#' + labelId;
                $(labelObject).hide('fast');
            }
        });
    });



    $(document).on('click', '.removeLabelLink', function () { // remove label
        var cardId = $('#CardId').val();
        var labelId = $(this).parent().prev().children().first().attr('id');
        var verificationToken = $('#__AjaxAntiForgeryForm input').val();
        var data = {
            __RequestVerificationToken: verificationToken,
            CardId: cardId,
            LabelId: labelId
        };

        $.ajax({
            url: "../../Labels/Delete",
            type: "POST",
            data: data,
            success: function (data) {
                $("#label-area").html(data);
                ReloadCards();
            }
        });
    });

});

