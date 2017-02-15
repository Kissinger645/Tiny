using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Tiny.Models;

namespace Tiny.Controllers
{
    public class uController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: u
        public ActionResult Index()
        {
            return View();
        }

        [Route("u/{userName}")]
        public ActionResult Index(string userName)
        {
            ApplicationUser user = db.Users.Where(u => u.UserName == userName).FirstOrDefault();
            string userId = user.Id;

            ViewBag.UserLinks = db.Links.Where(l => l.Owner.UserName == userName).ToList();

            return View(user);
        }
    }
}