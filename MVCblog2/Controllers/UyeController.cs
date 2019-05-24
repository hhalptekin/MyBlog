using MVCblog2.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MVCblog2.Controllers
{
    public class UyeController : Controller
    {
        MVCBlogDB db = new MVCBlogDB();
        public ActionResult Index(int id)
        {
            var data = db.Uyes.Where(x => x.UyeID == id).FirstOrDefault();
            if (Convert.ToInt32(Session["uyeid"]) != data.UyeID)
            {
                return HttpNotFound();
            }

            return View(data);
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Uye model)
        {
            var data = db.Uyes.Where(x => x.KullaniciAdi == model.KullaniciAdi).SingleOrDefault();
            if (model.KullaniciAdi == data.KullaniciAdi && data.Email == model.Email && model.Sifre == data.Sifre)
            {
                Session["uyeid"] = data.UyeID;
                Session["kullaniciadi"] = data.KullaniciAdi;
                Session["yetkiid"] = data.YetkiID;
                return RedirectToAction("Index", "Home");
            }
            else
            {
                ViewBag.uyari = "Kullanıcı Adı, Şifre yada E-Mail adresinizi kontrol ediniz!!";
                return View();
            }

        }

        public ActionResult LogOut()
        {
            Session["uyeid"] = null;
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }

        public ActionResult Create()
        {

            return View();
        }
        [HttpPost]
        public ActionResult Create(Uye model, HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {

                if (Foto != null)
                {
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(Foto.FileName);

                    string newFoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(150, 150);
                    img.Save("~/Upload/UyeFoto/" + newFoto);
                    model.Foto = "/Upload/UyeFoto/" + newFoto;
                    model.YetkiID = 2;
                    db.Uyes.Add(model);
                    db.SaveChanges();
                    Session["uyeid"] = model.UyeID;
                    Session["kullaniciadi"] = model.KullaniciAdi;
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.uyari = "Lütfen Tüm Alanları Doldurunuz!!!";
                    ModelState.AddModelError("Fotoğraf", "Fotoğraf Seçiniz.");
                }
            }
            return View();
        }

        public ActionResult Edit(int id)
        {
            var data = db.Uyes.Where(x => x.UyeID == id).FirstOrDefault();
            if (Convert.ToInt32(Session["uyeid"]) != data.UyeID)
            {
                return HttpNotFound();
            }
            else
            {
                return View(data);
            }


        }
        [HttpPost]
        public ActionResult Edit(Uye model, int id, HttpPostedFileBase Foto)
        {
            if (ModelState.IsValid)
            {
                var data = db.Uyes.Where(x => x.UyeID == id).FirstOrDefault();
                if (Foto != null)
                {
                    if (System.IO.File.Exists(Server.MapPath(model.Foto)))
                    {
                        System.IO.File.Delete(Server.MapPath(data.Foto));
                    }
                    WebImage img = new WebImage(Foto.InputStream);
                    FileInfo fotoinfo = new FileInfo(Foto.FileName);

                    string newFoto = Guid.NewGuid().ToString() + fotoinfo.Extension;
                    img.Resize(150, 150);
                    img.Save("~/Upload/UyeFoto/" + newFoto);
                    data.Foto = "/Upload/UyeFoto/" + newFoto;

                }
                data.AdSoyad = model.AdSoyad;
                data.KullaniciAdi = model.KullaniciAdi;
                data.Sifre = model.Sifre;
                data.Email = model.Email;

                db.SaveChanges();

                Session["kullaniciadi"] = model.KullaniciAdi;

                return RedirectToAction("Index", "Home", new { id = data.UyeID });

            }
            return View();
        }

        public ActionResult UyeProfil(int id)
        {
            var data = db.Uyes.Where(X => X.UyeID == id).FirstOrDefault();
            return View(data);
        }
    }
}