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
    public class TicketsController : Controller
    {
        private DataModel db = new DataModel();


        [HttpPost]
        [ValidateAntiForgeryToken]
        public void Create([Bind(Include = "Name,BoardId")] Ticket ticket)
        {

            int latestPoistion = (from c in db.Tickets
                                  where c.BoardId == ticket.BoardId
                                  orderby c.PositionNo descending
                                  select c.PositionNo).FirstOrDefault();

           
            ticket.CreatedOn = DateTime.Now;
            ticket.PositionNo = latestPoistion + 1;
            db.Tickets.Add(ticket);
            db.SaveChanges();           
        }



        [HttpPost]
        public HttpStatusCode UpdatePosition(List<Ticket> data)
        {
            var boardId = data[0].BoardId;
            var tickets = db.Tickets.Where((i) => i.BoardId == boardId).ToList();

            foreach (Ticket t in data)
            {
                var ticket = tickets.First((i) => i.Id == t.Id);
                ticket.PositionNo = t.PositionNo;
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
            }
            return HttpStatusCode.OK;
        }


        // POST: Tickets/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CreatedOn,BoardId")] Ticket ticket)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ticket).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BoardId = new SelectList(db.Boards, "Id", "Name", ticket.BoardId);
            return View(ticket);
        }


        // POST: Tickets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "ticketId")]int ticketId)
        {
            try
            {
                Ticket ticket = db.Tickets.Find(ticketId);
                db.Tickets.Remove(ticket);
                db.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.NotModified);
            }
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
