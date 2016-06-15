using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Trello;
using Trello.DAL;
using Trello.DAL.Models;
using WebGrease.Css.Extensions;

namespace Trello.Controllers
{
    [Authorize]
    public class BoardsController : Controller
    {
        private DataModel db = new DataModel();

        // GET: Boards
        public ActionResult Index()
        {

            //using (var context = new DataModel())
            //{
            //    var board = context.Boards.Find(1);
            //    context.Entry(board).Collection("Tickets").Load();
            //}

            var userId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            var userBoardsIds = db.UserBoards.Where((i) => i.UserId == userId).Select((i) => i.BoardId);
            var userBoards = db.Boards.Where((i) => userBoardsIds.Contains(i.Id));
            var ownedBoards = db.Boards.Where((i) => i.UserId == userId);

            var allBoards = userBoards.Union(ownedBoards).ToList();

            return View(allBoards);
        }

        // GET: Boards/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Board board = db.Boards.Find(id);
            var tickets = db.Tickets.Where(i => i.BoardId == board.Id).OrderBy((j) => j.PositionNo).ToList();
            
            tickets.ForEach((i)=>
            {
                i.Cards = i.Cards.OrderBy((j)=>j.PositionNo).ToList();
            });


            if (board == null)
            {
                return HttpNotFound();
            }
            board.Tickets = tickets.ToList();
            return View(board);
        }


        // POST: Boards/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name")] Board board)
        {
            board.UserId = System.Web.HttpContext.Current.User.Identity.GetUserId();
            board.CreatedOn = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Boards.Add(board);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(board);
        }

        // GET: Boards/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Board board = db.Boards.Find(id);
            if (board == null)
            {
                return HttpNotFound();
            }
            return View(board);
        }

        // POST: Boards/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,CreatedOn,UserId")] Board board)
        {
            if (ModelState.IsValid)
            {
                db.Entry(board).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(board);
        }

 
        // POST: Boards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "boardId")]int boardId)
        {
            try
            {
                Board board = db.Boards.Find(boardId);
                db.Boards.Remove(board);
                db.SaveChanges();
                return new HttpStatusCodeResult(HttpStatusCode.OK);
            }
            catch (Exception)
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
