using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Trello;
using Trello.DAL;
using Trello.DAL.Models;
using Trello.Helpers;
using Trello.Models;

namespace Trello.Controllers
{
    public class CardsController : AsyncController
    {
        private DataModel db = new DataModel();

        // GET: Cards/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Card card = db.Cards.SingleOrDefault((i)=>i.Id == id);
            if (card == null) return HttpNotFound();

            

            var taskItems = Task.Factory.StartNew(() => db.TaskItems.Where((i) => i.CardId == card.Id));
            var cardLabels = Task.Factory.StartNew(() => db.CardLabels.Where((e) => e.CardId == id));
            var comments = Task.Factory.StartNew(() => db.Comments.Where((i) => i.CardId == card.Id));
            var attachments = Task.Factory.StartNew(() => db.Attachments.Where((i) => i.CardId == card.Id));
            var labels = db.Labels.ToList();


            await Task.WhenAll(taskItems, cardLabels, comments,attachments); // release main thread to avoid thred starvation

            ViewBag.Card = card;
            ViewBag.TaskItems = taskItems.Result.ToList();
            ViewBag.Comments = comments.Result.ToList();
            ViewBag.CardLabels = cardLabels.Result.ToList();
            List<Label> labelToRemove = (from label in labels from cardLabel in cardLabels.Result.ToList() where label.Id == cardLabel.Label.Id select cardLabel.Label).ToList();
            ViewBag.Labels = labels.Except(labelToRemove);
            ViewBag.Attachments = attachments.Result.ToList();

            return PartialView("_CardDetails");
        }


        // POST: Cards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,TicketId")] Card card)
        {
            int latestPoistion = (from c in db.Cards
                                 where c.TicketId == card.TicketId
                                 orderby c.PositionNo descending
                                 select c.PositionNo).FirstOrDefault();


            if (ModelState.IsValid)
            {
                card.CreatedOn = DateTime.Now;
                card.PositionNo = latestPoistion+1;
                db.Cards.Add(card);
                db.SaveChanges();
            }

            var cards = db.Cards.Where((i) => i.TicketId == card.TicketId);
               
            ViewBag.Cards = cards.ToList();
                return PartialView("_CardList");
            }


        [HttpPost]
        public ActionResult GetCards(int ticketId)
            {
            var cards = db.Cards.Where((i) => i.TicketId == ticketId);

            ViewBag.Cards = cards.OrderBy((i)=>i.PositionNo).ToList();
            return PartialView("_CardList");
        }


        // POST: Cards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Card card = db.Cards.Find(id);
            db.Cards.Remove(card);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        [HttpPost]
        public HttpStatusCode UpdatePosition(List<Card> data)
        {
            var cards = db.Cards;

            foreach (Card c in data)
            {
                var card = cards.First((i) => i.Id == c.Id);
                card.TicketId = c.TicketId;
                card.PositionNo = c.PositionNo;

                db.Entry(card).State = EntityState.Modified;
                db.SaveChanges();
            }
            return HttpStatusCode.OK;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public string UpdateDate([Bind(Include = "date, cardId")] string date, int cardId)
        {
            var card = db.Cards.SingleOrDefault((c) => c.Id == cardId);
            DateTime rightDate = DateTime.ParseExact(date, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            card.DueDate = rightDate;
            db.SaveChanges();

            ViewBag.Card = card;
            return card.DueDate.ToString();
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
