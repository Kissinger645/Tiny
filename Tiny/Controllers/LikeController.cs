using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Tiny.Models;

namespace Tiny.Controllers
{
    public class LikeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Like
        public ActionResult Index()
        {
            var likes = db.Likes.Include(l => l.Liker).Include(l => l.Link);
            return View(likes.ToList());
        }

        // GET: Like/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Like like = db.Likes.Find(id);
            if (like == null)
            {
                return HttpNotFound();
            }
            return View(like);
        }

        // GET: Like/Create
        public ActionResult Create()
        {
            ViewBag.LikerId = new SelectList(db.Likes, "Id", "Email");
            ViewBag.LinkId = new SelectList(db.Links, "Id", "Title");
            return View();
        }

        // POST: Like/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,LikerId,LinkId")] Like like)
        {
            if (ModelState.IsValid)
            {
                db.Likes.Add(like);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LikerId = new SelectList(db.Likes, "Id", "Email", like.LikerId);
            ViewBag.LinkId = new SelectList(db.Links, "Id", "Title", like.LinkId);
            return View(like);
        }

        // GET: Like/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Like like = db.Likes.Find(id);
            if (like == null)
            {
                return HttpNotFound();
            }
            ViewBag.LikerId = new SelectList(db.Likes, "Id", "Email", like.LikerId);
            ViewBag.LinkId = new SelectList(db.Links, "Id", "Title", like.LinkId);
            return View(like);
        }

        // POST: Like/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,LikerId,LinkId")] Like like)
        {
            if (ModelState.IsValid)
            {
                db.Entry(like).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LikerId = new SelectList(db.Likes, "Id", "Email", like.LikerId);
            ViewBag.LinkId = new SelectList(db.Links, "Id", "Title", like.LinkId);
            return View(like);
        }

        // GET: Like/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Like like = db.Likes.Find(id);
            if (like == null)
            {
                return HttpNotFound();
            }
            return View(like);
        }

        // POST: Like/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Like like = db.Likes.Find(id);
            db.Likes.Remove(like);
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
