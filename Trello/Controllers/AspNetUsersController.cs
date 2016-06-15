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
    public class AspNetUsersController : Controller
    {
        private DataModel db = new DataModel();

        // GET: AspNetUsers
        public List<AspNetUser> Index()
        {
            return db.AspNetUsers.ToList();
        }


        [HttpGet]
        public JsonResult GetAspNetUsersForBoard(int? id)
        {
            var userId = HttpContext.User.Identity.GetUserId();
            var ownerId = db.Boards.First((i) => i.Id == id).UserId;
            var members = db.UserBoards.Where((i) => i.BoardId == id).Select((i) => i.UserId); // already members of the userBoard, we don't want them

            var users = from obj in db.AspNetUsers.Where((i) => i.Id != userId  && i.Id != ownerId && !members.Contains(i.Id)) // netto list
                        select new { id = obj.Id, value = obj.FirstName + " " + obj.LastName };


            return Json((users), JsonRequestBehavior.AllowGet);
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
