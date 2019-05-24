using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MVCblog2.Models;
using PagedList;
using PagedList.Mvc;

namespace MVCblog2.Controllers
{
    public class AdminMakaleController : Controller
    {
        private MVCBlogDB db = new MVCBlogDB();

        // GET: AdminMakale
        public ActionResult Index(int Page=1)
        {
            var data = db.Makales.OrderByDescending(m => m.MakaleID).ToPagedList(Page, 7);

            return View(data);
        }

        // GET: AdminMakale/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = db.Makales.Find(id);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        // GET: AdminMakale/Create
        public ActionResult Create()
        {
            ViewBag.KategoriID = new SelectList(db.Kategoris, "KategoriID", "KategoriAdi");
            return View();
        }

        // POST: AdminMakale/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MakaleID,Baslik,Icerik,Foto,Tarih,KategoriID,UyeID,Okunma")] Makale makale)
        {

            if (ModelState.IsValid)
            {
             
                makale.UyeID = Convert.ToInt32(Session["uyeid"]);
                makale.Okunma = 1;
                db.Makales.Add(makale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.KategoriID = new SelectList(db.Kategoris, "KategoriID", "KategoriAdi", makale.KategoriID);
            return View(makale);
        }

        // GET: AdminMakale/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = db.Makales.Find(id);
            if (makale == null)
            {
                return HttpNotFound();
            }
            ViewBag.KategoriID = new SelectList(db.Kategoris, "KategoriID", "KategoriAdi", makale.KategoriID);
            return View(makale);
        }

        // POST: AdminMakale/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MakaleID,Baslik,Icerik,Foto,Tarih,KategoriID,UyeID,Okunma")] Makale makale)
        {
            if (ModelState.IsValid)
            {
                
                db.Entry(makale).State = EntityState.Modified;

                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.KategoriID = new SelectList(db.Kategoris, "KategoriID", "KategoriAdi", makale.KategoriID);
            return View(makale);
        }

        // GET: AdminMakale/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Makale makale = db.Makales.Find(id);
            if (makale == null)
            {
                return HttpNotFound();
            }
            return View(makale);
        }

        // POST: AdminMakale/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Makale makale = db.Makales.Find(id);
            db.Makales.Remove(makale);
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
