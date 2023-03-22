using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ZH.Dbf;

namespace ZH.Controllers
{
    public class EmployeController : Controller
    {
        // GET: Employe
        public ActionResult Index()
        {
            return View();
        }    
        public ActionResult EmployeeProfile()
        {
            var idd = (Int32)Session["id"];
            var db = new ZHEntities();
            var data = db.Employes.SingleOrDefault(f => f.id == idd);
            return View(data);
        }

        public ActionResult History()
        {
            
            var db = new ZHEntities();
            var lists = db.RequestServes.Where(d => d.status == "Distributed").Include("Request").Include("Employe").ToList();
            return View(lists);
        }   
        
        public ActionResult Requests()
        {
            
            var db = new ZHEntities();
            var lists = db.Requests.Where(r => r.status != "Distributed" && r.status != "cancelled" ).ToList();
            return View(lists);
        }
        public ActionResult CancelRequest(int? id)
        {
            var db = new ZHEntities();
            var data = (from d in db.Requests where d.id == id select d).SingleOrDefault();

            data.status = "cancelled";
            int rowsAffected = db.SaveChanges();
            if (rowsAffected > 0)
            {
                TempData["msg"] = "Request Cancelled";
                return RedirectToAction("Requests");
            }
            else
            {
                TempData["msg"] = "Failed to cancel";
                return RedirectToAction("Requests");
            }
        }

        public ActionResult AcceptRequest(int? id, string status)
        {
            if(status == "requested")
            {
                var db = new ZHEntities();
                var data = db.Requests.Where(d => d.id == id).Include("Restaurent").SingleOrDefault();
                var location = data.Restaurent.location;

                var availableEmployee = db.Employes.Where(e => e.location == location).ToList();
                ViewBag.RequestId = id.Value;
                return View(availableEmployee);
            }
            else
            {

                return RedirectToAction("UpdateRequest", new { idd = id });
            }
             
        }

        [HttpPost]
        public ActionResult AcceptRequest(RequestServe r)
        {
            var db = new ZHEntities();
            db.RequestServes.Add(r);

            int affecredRow = db.SaveChanges();
            if (affecredRow > 0)
            {
                var data = db.Requests.Where(f => f.id == r.requestId).SingleOrDefault();
                data.status = r.status;
                db.SaveChanges();
                TempData["msg"] = "Status Updated";
                return RedirectToAction("Requests");
            }
            else
            {
                TempData["msg"] = "Failed to Update";
                return View(r);
            }

        }
        public ActionResult UpdateRequest(int idd)
        {
          
           // ViewBag.RequestId = idd;
            var db = new ZHEntities();
            var data = db.RequestServes.Where(d => d.requestId == idd).SingleOrDefault();

            return View(data);
        }

        [HttpPost]
        public ActionResult UpdateRequest(RequestServe r)
        {
            var db = new ZHEntities();
            var data = db.RequestServes.Where(d => d.id == r.id).SingleOrDefault();
            data.status = r.status;
            int row = db.SaveChanges();
            if (row >0)
            {
                var dataa = db.Requests.Where(f => f.id == data.requestId).SingleOrDefault();
                dataa.status = data.status;
                db.SaveChanges();
                TempData["msg"] = "Successfully updated";
                return RedirectToAction("Requests");
            }
            else
            {
                TempData["msg"] = "Failed to Update";
                return RedirectToAction("Requests");
            }
           
        }

    }
}