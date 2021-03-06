﻿using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Tiny.Models;

namespace Tiny.Controllers
{
    [Authorize]
    public class LinkController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Link
        public ActionResult Index()
        {
            var userId = User.Identity.GetUserId();
            var links = db.Links.Include(b => b.Owner).Where(b => b.Owner.Id == userId);
            
            return View(links.ToList());
        }

        public ActionResult Likes()
        {
            var userId = User.Identity.GetUserId();
            var links = db.Likes.Include(b => b.Liker).Where(b => b.LikerId == userId);
            ViewBag.AllLinks = db.Links.Where(l => l.Public == true).OrderByDescending(link => link.Created);
            ViewBag.MyFavs = links.ToList();
            return View();
        }
        
        public ActionResult Like(int id)
        {
            var userId = User.Identity.GetUserId();
            
            bool liked = db.Likes.Where(l => l.LinkId == id && l.LikerId == userId).Any();
            ViewBag.liked = liked;
            if (liked == false)
            {
                var linkId = db.Links.Where(l => l.Id == id).FirstOrDefault().Id;
                Like like = new Like
                {
                    LikerId = userId,
                    LinkId = id

                };
                db.Likes.Add(like);
                db.SaveChanges();
            }
            else
            {
                var like = db.Likes
                    .Where(l => l.LinkId == id && l.LikerId == userId).FirstOrDefault();
                db.Likes.Remove(like);
                db.SaveChanges();
            }
            return RedirectToAction("Likes", "Link");
        }
             
        static string Encrypt256(string input)
        {
            string shortUrl;
            byte[] byteData = Encoding.ASCII.GetBytes(input);
            Stream inputStream = new MemoryStream(byteData);

            using (SHA256 shaM = new SHA256Managed())
            {
                var result = shaM.ComputeHash(inputStream);
                shortUrl = BitConverter.ToString(result);
            }
            return shortUrl.Replace("-", "").Substring(0, 5);
        }

        [Route("l/{ShortUrl}")]
        public ActionResult ReRoute(string shortUrl)
        {
            Link link = db.Links.Where(l => l.ShortUrl == shortUrl).FirstOrDefault();
            if (link == null)
            {
                return HttpNotFound();
            }
            Click click = new Click();
            var userId = User.Identity.GetUserId();
            var linkId = link.Id;
            click.LinkId = linkId;
            click.Clicks++;
            click.TimeStamp = DateTime.Now;
            db.Clicks.Add(click);
            db.SaveChanges();
            return new RedirectResult(link.Url);
        }

        // GET: Link/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Link link = db.Links.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            return View(link);
        }

        // GET: Link/Create
        public ActionResult Create()
        {
            ViewBag.OwnerId = new SelectList(db.Links, "Id", "Email");
            return View();
        }

        // POST: Link/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Title,Description,Url,ShortUrl,Created,Public,UserName,OwnerId")] Link link)
        {
            if (ModelState.IsValid)
            {
                link.UserName = User.Identity.GetUserName();
                link.ShortUrl = Encrypt256(link.Url);
                link.OwnerId = User.Identity.GetUserId();
                link.Created = DateTime.Now;
                db.Links.Add(link);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.OwnerId = new SelectList(db.Links, "Id", "Email", link.OwnerId);
            return View(link);
        }

        // GET: Link/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userId = User.Identity.GetUserId();
            Link link = db.Links
                .Where(b => b.OwnerId == userId)
                .Where(b => b.Id == id)
                .FirstOrDefault();
            if (link == null)
            {
                return HttpNotFound();
            }
            ViewBag.OwnerId = new SelectList(db.Links, "Id", "Email", link.OwnerId);
            return View(link);
        }

        // POST: Link/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Title,Description,Url,ShortUrl,Created,Public,UserName,OwnerId")] Link link)
        {
            if (ModelState.IsValid)
            {
                db.Entry(link).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.OwnerId = new SelectList(db.Links, "Id", "Email", link.OwnerId);
            return View(link);
        }

        // GET: Link/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Link link = db.Links.Find(id);
            if (link == null)
            {
                return HttpNotFound();
            }
            return View(link);
        }

        // POST: Link/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Link link = db.Links.Find(id);
            db.Links.Remove(link);
            db.SaveChanges();
            return RedirectToAction("Index");
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
