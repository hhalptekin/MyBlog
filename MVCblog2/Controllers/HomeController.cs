using MVCblog2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using PagedList.Mvc;

namespace MVCblog2.Controllers
{
    public class HomeController : Controller
    {
        MVCBlogDB db = new MVCBlogDB();
        public ActionResult Index(int Page = 1)
        {

            var data = db.Makales.OrderByDescending(m => m.MakaleID).ToPagedList(Page, 5);
            return View(data);
        }
        public ActionResult KategoriMakale(int id)
        {
            var data = db.Makales.Where(x => x.Kategori.KategoriID == id).ToList();
            return View(data);

        }
        public ActionResult Hakkimizda()
        {
            return View();
        }
        public ActionResult İletisim()
        {
            return View();
        }
        public ActionResult KategoriPartial()
        {
            var data = db.Kategoris.ToList();
            return View(data);
        }
        public ActionResult MakaleDetay(int id)
        {
            var data = db.Makales.Where(x => x.MakaleID == id).FirstOrDefault();
            if (data == null)
            {
                return HttpNotFound();
            }
            return View(data);
        }
        public JsonResult YorumYap(string yorum, int Makaleid)
        {
            var uyeid = Session["uyeid"];
            if (yorum != null)
            {
                db.Yorums.Add(new Yorum { UyeID = Convert.ToInt32(uyeid), MakaleID = Makaleid, İcerik = yorum, Tarih = DateTime.Now });
                db.SaveChanges();
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }
        public ActionResult YorumSil(int id)
        {
            var uyeid = Session["uyeid"];

            var yorum = db.Yorums.Where(x => x.YorumID == id).SingleOrDefault();

            var makale = db.Makales.Where(x => x.MakaleID == yorum.MakaleID).SingleOrDefault();

            if (Convert.ToInt32(uyeid) == yorum.UyeID)
            {
                db.Yorums.Remove(yorum);
                db.SaveChanges();
                return RedirectToAction("MakaleDetay", "Home", new { id = makale.MakaleID });
            }

            else
            {
                return HttpNotFound();
            }
        }
        public ActionResult OkumaArttir(int Makaleid)
        {
            var data = db.Makales.Where(x => x.MakaleID == Makaleid).FirstOrDefault();
            data.Okunma += 1;
            db.SaveChanges();
            return View();
        }
        public ActionResult BlogAra(string Ara = null)
        {

            var data = db.Makales.Where(x => x.Baslik.Contains(Ara)).ToList();
            return View(data.OrderByDescending(y => y.Tarih));

        }
        public ActionResult SonYorumlar()
        {
            var data = db.Yorums.OrderByDescending(y => y.YorumID).Take(5);
            return View(data);
        }
        public ActionResult EnCokOkunanMakaleler()
        {
            var data = db.Makales.OrderByDescending(y => y.Okunma).Take(5);
            return View(data);
        }
    }
}