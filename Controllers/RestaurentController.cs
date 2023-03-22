using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZH.Dbf;

namespace ZH.Controllers
{
    public class RestaurentController : Controller
    {
        // GET: Restaurent
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult RestaurentProfile()
        {
            var idd = (Int32)Session["id"];
            var db = new ZHEntities();
            var data = db.Restaurents.SingleOrDefault(f => f.id == idd);
            return View(data);
        }

        [HttpGet]
        public ActionResult NewRequest()
        {
            return View();
        }
        [HttpPost]
        public ActionResult NewRequest(Request r)
        {
            if(ModelState.IsValid)
            {
                var db = new ZHEntities();
                r.status = "requested";
                r.restaurentId =  (Int32)Session["id"];
                db.Requests.Add(r);
                int res = db.SaveChanges();

                if(res > 0)
                {
                    TempData["msg"] = "Request Submitted";
                    return RedirectToAction("Requests");
                }
                else
                {
                    TempData["msg"] = "failed to request";
                    return View(r);
                }
                
            }
            else
            {
                return View(r);
            }
           
        }

        public ActionResult Requests()
        {
            var idd = (Int32)Session["id"];
            var db = new ZHEntities();
            var lists = db.Requests.Where(r => r.restaurentId == idd)
                .OrderByDescending(r => r.id)
                .ToList();
            return View(lists);
        }


    }
}