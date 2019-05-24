using MVCblog2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCblog2.Controllers
{
    public class AdminController : Controller
    {
        MVCBlogDB db = new MVCBlogDB();
        public ActionResult Index()
        {

            ViewBag.MakaleSayisi = db.Makales.Count();           
            ViewBag.KategoriSayisi = db.Kategoris.Count();
            ViewBag.UyeSayisi = db.Uyes.Count();
            ViewBag.YorumSayisi = db.Yorums.Count();
            return View();
        }
    }
}