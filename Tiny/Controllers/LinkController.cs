using Microsoft.AspNet.Identity;
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
    public class LinkController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Link
        public ActionResult Index()
        {
            var links = db.Links.Include(l => l.Owner);
            ViewBag.MyLinks = links.OrderByDescending(b => b.Created).ToList();
            return View(links.ToList());
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
            Link link = db.Links.Find(id);
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
