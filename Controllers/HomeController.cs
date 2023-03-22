using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZH.Dbf;

namespace ZH.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
             
        public ActionResult Logout()
        {
            // Clear the session
            Session.Clear();
            TempData["msg"] = "log out successfully";
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet]
        public ActionResult RegEmploye()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegEmploye(Employe e)
        {
            if (ModelState.IsValid)
            {
                var db = new ZHEntities();
                db.Employes.Add(e);

                int rowsAffected = db.SaveChanges();
                if (rowsAffected > 0)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["msg"] = "failed to register";
                    return View(e);
                }

            }
            else
            {
                return View(e);
            }

        }

        [HttpGet]
        public ActionResult RegRestaurent()
        {
            return View();
        }
        [HttpPost]
        public ActionResult RegRestaurent(Restaurent r)
        {
            if (ModelState.IsValid)
            {
                var db = new ZHEntities();
                db.Restaurents.Add(r);
              int rowsAffected = db.SaveChanges();
                if (rowsAffected > 0)
                {
                    return RedirectToAction("Login");
                }
                else
                {
                    TempData["msg"] = "failed to register";
                    return View(r);
                }
               
            }
            else
            {
                return View(r);
            }
            
        }

        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }   
        [HttpPost]
        public ActionResult Login(FormCollection form)
        {
            string Email = form["email"];
            string Password = form["password"];

            if (Email != null && Password != null)
            {
                var db = new ZHEntities();

                var res = db.Employes.SingleOrDefault(e => e.email == Email && e.password == Password);
                var ress = db.Restaurents.SingleOrDefault(f => f.email == Email && f.password == Password);
                if (res != null)
                {
                    Session["user"] = res.name;
                    Session["id"] = res.id;
                    return RedirectToAction("Index", "Employe");
                }
                else if (ress != null)
                {
                    Session["user"] = ress.name;
                    Session["id"] = ress.id;
                    return RedirectToAction("Index", "Restaurent");
                }
                else
                {
                    TempData["msg"] = "Not matched";
                    return View();
                }
               
            }
            else
            {
                TempData["msg"] = "Enter Email/password";
                return View();
            }
            
        }
    }
}