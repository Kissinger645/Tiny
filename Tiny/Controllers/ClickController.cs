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
    public class ClickController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Click
        public ActionResult Index()
        {
            var clicks = db.Clicks.Include(c => c.Link);
            return View(clicks.ToList());
        }

        // GET: Click/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Click click = db.Clicks.Find(id);
            if (click == null)
            {
                return HttpNotFound();
            }
            return View(click);
        }

        // GET: Click/Create
        public ActionResult Create()
        {
            ViewBag.LinkId = new SelectList(db.Links, "Id", "Title");
            return View();
        }

        // POST: Click/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TimeStamp,Clicks,LinkId")] Click click)
        {
            if (ModelState.IsValid)
            {
                db.Clicks.Add(click);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.LinkId = new SelectList(db.Links, "Id", "Title", click.LinkId);
            return View(click);
        }

        // GET: Click/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Click click = db.Clicks.Find(id);
            if (click == null)
            {
                return HttpNotFound();
            }
            ViewBag.LinkId = new SelectList(db.Links, "Id", "Title", click.LinkId);
            return View(click);
        }

        // POST: Click/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TimeStamp,Clicks,LinkId")] Click click)
        {
            if (ModelState.IsValid)
            {
                db.Entry(click).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.LinkId = new SelectList(db.Links, "Id", "Title", click.LinkId);
            return View(click);
        }

        // GET: Click/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Click click = db.Clicks.Find(id);
            if (click == null)
            {
                return HttpNotFound();
            }
            return View(click);
        }

        // POST: Click/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Click click = db.Clicks.Find(id);
            db.Clicks.Remove(click);
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
