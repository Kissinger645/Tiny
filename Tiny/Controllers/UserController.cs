using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tiny.Models;

namespace Tiny.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: User
        public ActionResult Index()
        {
            ViewBag.AllLinks = db.Links.Where(l => l.Public == true).OrderByDescending(link => link.Created);
            return View();
        }
		
        [Route("u/{UserName}")]
        public ActionResult Detail(string UserName)
        {
            ApplicationUser user = db.Users.Where(u => u.UserName == UserName).FirstOrDefault();
            string userId = user.Id;

            ViewBag.UserLinks = db.Links.Where(l => (l.OwnerId == userId) &&
            (l.Public == true))
            .ToList();

            return View(user);
        }
    }
}