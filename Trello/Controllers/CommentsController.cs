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
using Trello.Helpers;
using Trello.Models;

namespace Trello.Controllers
{
    public class CommentsController : Controller
    {
        private DataModel db = new DataModel();


        // POST: Comments/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Text,CreatedOn,CardId")] Comment comment)
        {
            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            comment.AspNetUserId = userId;
            comment.CreatedOn = DateTime.Now;

            db.Comments.Add(comment);
            db.SaveChanges();


            var comments = from com in db.Comments.Include((i) => i.CommentReplies).Include((i) => i.AspNetUser)
                           where com.CardId == comment.CardId
                           orderby com.CreatedOn descending
                           select com;



            ViewBag.Comments = comments.ToList();
            return PartialView("_CommentsList");
        }


        // POST: Comments/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Text,Id")] Comment comment)
        {
            var _comment = db.Comments.First((i) => i.Id == comment.Id);
            _comment.Text = comment.Text;

            db.Entry(_comment).State = EntityState.Modified;
            db.SaveChanges();

            var comments = from com in db.Comments.Include((i) => i.CommentReplies).Include((i) => i.AspNetUser)
                           where com.CardId == _comment.CardId
                           orderby com.CreatedOn descending
                           select com;


            ViewBag.Comments = comments.ToList();
            return PartialView("_CommentsList");
        }

        // POST: Comments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Comment comment = db.Comments.Find(id);
            db.Comments.Remove(comment);
            db.SaveChanges();

            var comments = from com in db.Comments.Include((i) => i.CommentReplies).Include((i) => i.AspNetUser)
                           where com.CardId == comment.CardId
                           orderby com.CreatedOn descending
                           select com;

            ViewBag.Comments = comments.ToList();
            return PartialView("_CommentsList");
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
