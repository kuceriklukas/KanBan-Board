using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Trello;
using Trello.DAL.Models;

namespace Trello.Controllers
{
    public class LabelsController : Controller
    {
        private DataModel db = new DataModel();

        [HttpPost, ActionName("ReloadLabelsDialog")]
        [ValidateAntiForgeryToken]
        public ActionResult ReloadLabelsDialog([Bind(Include = "id")] int id)
        {
            var cardLabels = db.CardLabels.Where((e) => e.CardId == id).ToList();
            var labels = db.Labels.ToList();
            List<Label> labelToRemove = (from label in labels from cardLabel in cardLabels where label.Id == cardLabel.Label.Id select cardLabel.Label).ToList();

            ViewBag.Labels = labels.Except(labelToRemove).ToList();
            return PartialView("_AddLabel");
        }

        // GET: Labels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Label label = db.Labels.Find(id);
            if (label == null)
            {
                return HttpNotFound();
            }
            return View(label);
        }

        // GET: Labels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Labels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "CardId, LabelId")] int cardId, int labelId)
        {
            if (ModelState.IsValid)
            {
                //var cardLabel = db.CardLabels.SingleOrDefault((c) => c.CardId == cardId);
                //var dbLabel = db.Labels.SingleOrDefault((c) => c.ColorTag.Equals(labelId));

                
                CardLabel cardLabel = new CardLabel() { CardId = cardId, LabelId = labelId };
                db.CardLabels.Add(cardLabel);
                
                db.SaveChanges();

                ViewBag.CardLabels = db.CardLabels.Where((e) => e.CardId == cardId).Include((i) => i.Label);
                return PartialView("_LabelDetails");
            }

            return PartialView("_LabelDetails");
        }

        // GET: Labels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Label label = db.Labels.Find(id);
            if (label == null)
            {
                return HttpNotFound();
            }
            return View(label);
        }

        // POST: Labels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,ColorTag")] Label label)
        {
            if (ModelState.IsValid)
            {
                db.Entry(label).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(label);
        }

        // GET: Labels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Label label = db.Labels.Find(id);
            if (label == null)
            {
                return HttpNotFound();
            }
            return View(label);
        }

        // POST: Labels/Delete/5
        [HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "CardId, LabelId")] int cardId, int labelId)
        {
            var cardLabel = db.CardLabels.SingleOrDefault((c) => (c.CardId == cardId) && (c.Label.Id == labelId));
            db.CardLabels.Remove(cardLabel);
            db.SaveChanges();
            ViewBag.CardLabels = db.CardLabels.Where((e) => e.CardId == cardId).Include((i) => i.Label);
            return PartialView("_LabelDetails");
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
