var attachmentId = null;

function SendAttachment() { // Create attachment
    var foda = new FormData(document.getElementById("attachmentFormUpload")); // returns HTML DOM object, not $ object!
   
    $.ajax({
        url: "/Attachments/Create",
        data: foda,
        cache: false,
        contentType: false,
        processData: false,
        type: "POST",
        success: function (data) {
            $("#attachement_area").html(data);
            ReloadCards();
        }
    });
}

$(document).ready(function () {


    $(document).on('contextmenu', '.download_image', function (event) { // display context menu
        $("#contexmennu").hide();
        event.stopPropagation();
        event.preventDefault();

        $("#contexmennu").css({ "top": event.pageY - 55 + "px", "left": event.pageX }).show();
        attachmentId = $(this).attr('id');
    });






    $(document).on('click', '#contexmennu', function () { // click delete context menu
        if (attachmentId == null) {
            alert("Attachment id not set!");
            return;
        }

        var token = $("#__AjaxAntiForgeryForm input").val();
        var data = {
            __RequestVerificationToken: token,
            id: attachmentId,
            cardId: $("#CardId").val()
        };

        $.ajax({
            url: "/Attachments/Delete",
            type: "POST",
            data: data,
            success: function (data) {
                $("#attachement_area").html(data);
                $("#contexmennu").hide();
                attachmentId = null;
                ReloadCards();
            },
            error: function() {
                attachmentId = null;
                $("#contexmennu").hide();
            }
        });
    });

});


$(document).bind("mousedown", function (event) { // hide if clicked elsewhere
    if ($(event.target.closest('.download_image')).length === 0 &&
        $(event.target.closest('#contexmennu')).length === 0) {
        $("#contexmennu").hide();
    }
});