var isReply = false;
var replyCommentId = null;

$(function () {

    $(document).on('click', '#btn_addComment', function () { // Add new comment / Add new reply

       if ($("#comment_text").val() === "") return;

       var text = $("#comment_text").val();
       var cardId = $("#CardId").val();
       var token = $("#__AjaxAntiForgeryForm input").val();

       if (!isReply) addComment(token, text, cardId); // new comment
       if (isReply) addReply(token, text, cardId); // new reply

    });




    function addReply(token, text, cardId) // Add reply
    {
        var data = {
            __RequestVerificationToken: token,
            Text: text,
            CommentId: replyCommentId,
            CardId:cardId
        };


        $.ajax({
            url: "/CommentReplies/Create",
            type: "POST",
            data: data,
            success: function (data) {
                $("#comments").html(data);
                $("#comment_text").val("");
                isReply = false;
            }
        });
    }





    function addComment(token, text, cardId) // Add new comment
    {
        var data = {
            __RequestVerificationToken: token,
            CardId: cardId,
            Text: text
        };

        $.ajax({
            url: "/Comments/Create",
            type: "POST",
            data: data,
            success: function (data) {
                $("#comments").html(data);
                $("#comment_text").val("");
                isReply = false;
                ReloadCards(); // reload cards to show comment icon
            }
        });
    }







    $(window).click(function (event) { // handle click outside edit links
        if ($(event.target.closest('.btn_updateComment')).length !== 1
        && $(event.target.closest('.edit_comment')).length !== 1
        && $(event.target.closest('.edit_reply_comment')).length !== 1
        && $(event.target.closest('.comment_textarea')).length !== 1
            ) {
            $('.edit_comment_btns, .edit_reply_comment_btns').hide();
            $('.comment_links').show();
            $('.comment_textarea').removeClass('to_white')
                                  .addClass("to_gray")
                                  .attr("contentEditable", false);
        }
    });




    $(document).on('click', '.edit_comment', function () { // click edit comment link

        isReply = false;
        $(".edit_comment_btns, .edit_reply_comment_btns").hide();

        $(".comment_links").show();
        $('.comment_textarea').removeClass('to_white')
                              .addClass("to_gray");

        var $comment = $(this).closest('.comment');


        $comment.find('.comment_textarea:first')
                .attr("contentEditable", true)
                .focus()
                .removeClass('to_gray')
                .addClass('to_white');

        $comment.find(".comment_links").hide();
        $comment.find(".edit_comment_btns").show();
    });





    $(document).on('click', '.close', function() { // close button 
        $(this).closest('.comment').find('.comment_textarea')
                                   .attr("contentEditable", false)
                                   .removeClass('to_white')
                                   .addClass('to_gray');

        $(this).closest('.comment').find(".comment_links").show();
        $(".edit_comment_btns").hide();  // hide comment buttons
        $(".edit_reply_comment_btns").hide(); // hide comment reply buttons
    });






    $(document).on('click', '.delete_comment', function () { // Delete comment

        isReply = false;
        var commentId = $(this).closest(".comment").attr('id').replace(/[^0-9]/g, '');
        var token = $("#__AjaxAntiForgeryForm input").val();
        var data = {
            __RequestVerificationToken: token,
            Id: commentId
        };

        $.ajax({
            url: "/Comments/Delete",
            type: "POST",
            data: data,
            success: function (data) {
                $("#comments").html(data);
                $("#comment_text").val("");
                ReloadCards();
            }
        });
    });







    $(document).on('click', '.reply_comment_link', function () { // reply comment link

        isReply = true;
        replyCommentId = $(this).closest(".comment").attr('id').replace(/[^0-9]/g, ''); 

        $('html, #CardDetailModal').animate({
            scrollTop: $(".add_comment").offset().top
        }, 500, function () {
            $('.add_comment').val("@Reply: ").focus();
        });


    });






    $(document).on('click', '.reply_on_reply_comment_link', function () { // reply on reply

        var user = $(this).closest('.reply_comment').find('.userFullName').val();
        replyCommentId = $(this).closest(".comment").attr('id').replace(/[^0-9]/g, '');
        isReply = true;

        $('html, #CardDetailModal').animate({
            scrollTop: $(".add_comment").offset().top
        }, 500, function () {
            $('.add_comment').val("@"+user+" ").focus();
        });
    });






    $(document).on('click', '.btn_updateComment', function () { // Edit comments

        var commentId = $(this).closest(".comment").attr('id').replace(/[^0-9]/g, '');
        var $comment = $(this).closest('.comment');

        var text = $comment.find('.comment_textarea:first').text();
        var token = $("#__AjaxAntiForgeryForm input").val();

        var $buttons = $comment.find('.edit_comment_btns');
        var $links = $comment.find('.comment_links');

        var data = {
            __RequestVerificationToken: token,
            Id: commentId,
            Text:text
        };

        $.ajax({
            url: "/Comments/Edit",
            type: "POST",
            data: data,
            success: function (data) {
                $("#comments").html(data);
                $("#comment_text").val("");
                $buttons.hide();
                $links.show();
            }
        });
    });


    $(document).on('click', '.delete_reply_comment', function() {
        var replyCommentId = $(this).closest(".reply_comment").attr('id').replace(/[^0-9]/g, '');
        var cardId = $("#CardId").val();
        var token = $("#__AjaxAntiForgeryForm input").val();

        var data = {
            __RequestVerificationToken: token,
            id: replyCommentId,
            cardId: cardId
        };

        $.ajax({
            url: "/CommentReplies/Delete",
            type: "POST",
            data: data,
            success: function (data) {
                $("#comments").html(data);
            }
        });

    });










    
    $(document).on('click', '.btn_updateCommentReply', function () { // create comment reply 

        $(".edit_comment_btns, .edit_reply_comment_btns").hide(); 
        $('.comment_links').show();

        var token = $("#__AjaxAntiForgeryForm input").val();
        var text = $(this).closest('.reply_comment').find('.comment_textarea').text();
        var replyCommentId = $(this).closest(".reply_comment").attr('id').replace(/[^0-9]/g, '');
        var cardId = $("#CardId").val();


        var data = {
            __RequestVerificationToken: token,
            Id: replyCommentId,
            Text: text,
            CardId:cardId
        };

       
        $.ajax({
            url: "/CommentReplies/Edit",
            type: "POST",
            data: data,
            success: function (data) {
                $("#comments").html(data);
            }
        });

    });







    $(document).on('click', '.edit_reply_comment', function () { // Click edit reply comment

        $('.comment_textarea').removeClass('to_white')
                             .addClass("to_gray");

        $(".edit_comment_btns, .edit_reply_comment_btns").hide(); // first hide all buttons
        $('.comment_links').show(); // and show all links


        $(this).closest('.comment_links').hide();
        $(this).closest('.reply_comment').find('.edit_reply_comment_btns').show();

        $(this).closest('.reply_comment').find('.comment_textarea')
                                .attr("contentEditable", true)
                                .removeClass('to_gray')
                                .addClass('to_white');
                               
      


    });

});