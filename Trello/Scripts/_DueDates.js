function ChooseDateEvent() {
    
    $('#datepicker').datepicker({
        inline: true,
        showOtherMonths: true,
        dayNamesMin: ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'],
        //onSelect: function() {
        //    date = $('#datepicker').datepicker('getDate');
        //    $('#timePicker').timepicker();           
        //    console.log(date);
        //}
    });

    $('#sendDateButton').click(function () {
        var myDate = $('#datepicker').datepicker().val();
        var splitted = myDate.split('/');

        var CardId = $("#CardId").val();
        var time = $('#timePicker').val();
        var strippedTime = time.substring(1);

        var day = splitted[1];       
        var month = splitted[0];    
        var year = splitted[2]; 
        var hour = strippedTime;     
        var minute = '00'; 
        var second = '00';

        var dateTimeString = day + "/" + month + "/" + year + " " + hour + ':' + minute + ':' + second;

        var token = $('#__AjaxAntiForgeryFormDueDates input').val();

        var data = {
            __RequestVerificationToken: token,
            date: dateTimeString,
            cardId: CardId
        };

        $.ajax({
            url: "/Cards/UpdateDate",
            type: "POST",
            data: data,
            success: function (data) {
                $("#cardDueDate").html(data);
                ReloadCards();
            }
        });
    });
};

$(function () {
    $(document).on('click', '#chooseDateButton', function() {
        //if ($('#datepicker').is(':hidden')) {
        //    $('#datepicker').show();
        //} else {
        //    $('#datepicker').hide();
        //}
        $('#timePickerModal').modal('show');
    });

});