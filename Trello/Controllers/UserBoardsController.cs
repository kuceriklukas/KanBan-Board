using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Trello;
using Trello.DAL.Models;

namespace Trello.Controllers
{
    public class UserBoardsController : AsyncController
    {
        private DataModel db = new DataModel();

        // GET: UserBoards
        public async Task<ActionResult> Index(int id)
        {
            var ownerId = db.Boards.First((i) => i.Id == id).UserId;

            Task<IQueryable<UserBoard>> userBoards =  Task.Factory.StartNew(
               ()=>  db.UserBoards.Where((i) => i.BoardId == id).Include(u => u.AspNetUser).Include(u => u.Board)
            );


            await Task.WhenAll(userBoards);

            ViewBag.OwnerId = ownerId;
            ViewBag.BoardId = id;
            ViewBag.UserBoards = userBoards.Result.ToList();
            return View();
        }




        // POST: UserBoards/Create
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "BoardId,UserId")] UserBoard userBoard)
        {
            userBoard.Id = new Random().Next();
            if (ModelState.IsValid)
            {
                db.UserBoards.Add(userBoard);
                db.SaveChanges();
                return RedirectToAction("Index", new {id = userBoard.BoardId});
            }

            return new HttpNotFoundResult("Failed");
        }



        // POST: UserBoards/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            UserBoard userBoard = db.UserBoards.Find(id);
            var boardId = userBoard.BoardId;
            db.UserBoards.Remove(userBoard);
            db.SaveChanges();
            return RedirectToAction("Index", "UserBoards" ,new {id = boardId});
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
