using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Trello.DAL.Models;

namespace Trello.Controllers
{
    public class AttachmentsController : Controller
    {
        private DataModel db = new DataModel();


        // POST: Attachments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Create(HttpPostedFileBase upload, int CardId)
        {

            byte[] byteContent = null;
            using (var reader = new System.IO.BinaryReader(upload.InputStream))
            {
                byteContent = reader.ReadBytes(upload.ContentLength);
            }
           
            var attachment = new Attachment
            {
                CardId = CardId,
                CreatedOn = DateTime.Now,
                FileName = System.IO.Path.GetFileName(upload.FileName),
                FileContent = byteContent
            };


            if (ModelState.IsValid)
            {
                db.Attachments.Add(attachment);
                db.SaveChanges();
            }
          
            var attachments = db.Attachments.Where((i) => i.CardId == CardId).ToList();
            ViewBag.Attachments = attachments;

            return PartialView("_AttachmentList");
        }




        // POST: Attachments/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, int cardId)
        {
            Attachment attachment = db.Attachments.Find(id);
            db.Attachments.Remove(attachment);
            db.SaveChanges();
            ViewBag.Attachments = db.Attachments.Where((i) => i.CardId == cardId).ToList();
            return PartialView("_AttachmentList");
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
