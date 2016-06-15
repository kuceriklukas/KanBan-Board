using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Trello;
using Trello.DAL.Models;

namespace Trello.Controllers
{
    public class CommentRepliesController : Controller
    {
        private DataModel db = new DataModel();



        // POST: CommentReplies/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Text,CommentId, CardId")] CommentReply commentReply, int cardId)
        {
            commentReply.CreatedOn = DateTime.Now;

            if (commentReply.Text.Contains("@Reply:")) commentReply.Text = commentReply.Text.Replace("@Reply:", "");
            else commentReply.Text = commentReply.Text;
            

            commentReply.AspNetUserId = HttpContext.User.Identity.GetUserId();

            db.CommentReplies.Add(commentReply);
            db.SaveChanges();

            ViewBag.Comments = db.Comments
                                          .Where((i) => i.CardId == cardId)
                                          .OrderByDescending((i) => i.CreatedOn).Include((i) => i.CommentReplies.Select((j) => j.AspNetUser)).ToList();
                                        

            return PartialView("../../Views/Comments/_CommentsList");
            
        }


        // POST: CommentReplies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Text,Id,CardId")] CommentReply commentReply, int cardId)
        {
            var _comReply = db.CommentReplies.FirstOrDefault((i) => i.Id == commentReply.Id);
            if(_comReply == null) return new HttpNotFoundResult("Failed to update");
            _comReply.Text = commentReply.Text;

            db.Entry(_comReply).State = EntityState.Modified;
            db.SaveChanges();

            ViewBag.Comments = db.Comments
                                          .Where((i) => i.CardId == cardId)
                                          .OrderByDescending((i) => i.CreatedOn).Include((i) => i.CommentReplies.Select((j) => j.AspNetUser)).ToList();

            return PartialView("../../Views/Comments/_CommentsList");
        }



        // POST: CommentReplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, int cardId)
        {
            CommentReply commentReply = db.CommentReplies.Find(id);
            db.CommentReplies.Remove(commentReply);
            db.SaveChanges();
            ViewBag.Comments = db.Comments
                              .Where((i) => i.CardId == cardId)
                              .OrderByDescending((i) => i.CreatedOn).Include((i) => i.CommentReplies.Select((j) => j.AspNetUser)).ToList();

            return PartialView("../../Views/Comments/_CommentsList");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
