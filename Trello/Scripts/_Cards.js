// JS for Cards

$(function() {

    // show modal with cards details on card click 
    $(document).on('click', '.card', function() {
        var cardId = $(this).attr('id');
        var strippedId = cardId.substring(1);
        $('.ui-dialog').remove(); // remove the label dialog from previous closed card if it's there
        $("#CardDetailModal").html("").load("/Cards/Details/" + strippedId).modal('show');
    });


    // add a card link
    $(document).on('click', '.addCard', function() {
        
        var htmlCode = '<div class="card new"><textarea name="card" placeholder="Enter note..."></textarea></div>';

        $(this).closest('.ticket').find('.cardsBoxContainer').hide();
        $(this).closest('.ticket').find('.cardsBox').append(htmlCode);
        $(this).closest('.ticket').find('.card_footer').show();
    });


    //Close button in footer
    $(document).on('click', '.close', function() {
        $(this).closest(".ticket").find(".card.new").remove();
        $(this).closest('.ticket').find(".card_footer").hide();
        $(this).closest('.ticket').find('.cardsBoxContainer').show();
    });





    //create new card
    $(document).on('click', '.btn_addCard:not(.withTicket)', function () {

        var $this = $(this);
        var textArea = $this.closest('.ticket').find('textarea').val();
        var TicketId = $this.closest('.ticket').attr('id');
        var $cardList = $this.closest('.ui-state-default').find('.cardslist');

        var token = $("#__AjaxAntiForgeryForm input").val();
        var data = {
            __RequestVerificationToken: token,
            Name: textArea,
            TicketId: TicketId
        };

        $.ajax({
            url: "/Cards/Create",
            type: "POST",
            data: data,
            success: function(data) {

                $cardList.html(data);

                $this.closest('.ticket').find('.cardsBoxContainer').show();
                $this.closest('.ticket').find('.card_footer').hide();
            }
        }).done(function() { // reattach sortable event to dynamically created elements
            AttachCardSortable();
        });;

    });
});


function ReloadCards() //Reload cards area
{
    var ticketId = $('input[name=TicketId]').val();
    $('.ticket[id=' + ticketId + '] .cardslist').load("/Cards/GetCards", { ticketId: ticketId }, function() {
        AttachCardSortable();
    });
    
}


