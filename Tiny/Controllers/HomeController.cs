﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tiny.Models;

namespace Tiny.Controllers
{
    public class HomeController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            ViewBag.AllLinks = db.Links.Where(l => l.Public == true).OrderByDescending(link => link.Created);
            return View();
        }

        [HttpPost]
        public ActionResult Like(int linkid)
        {
            Link link = db.Links.Find(linkid);
            if (link == null)
            {
                return HttpNotFound();
            }
            var userId = User.Identity.GetUserId();
            var linkId = db.Links.Where(l => l.Id == linkid).FirstOrDefault().Id;

            Like like = new Like
            {
                LikerId = userId,
                LinkId = linkId

            };
            db.Likes.Add(like);
            db.SaveChanges();
            return RedirectToAction("Likes");
        }

    }
}