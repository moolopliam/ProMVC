﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Khruphanth.Models;

namespace Khruphanth.Controllers
{
    public class HomeController : Controller
    {
        private ComCSDBEntities db = new ComCSDBEntities();
        public ActionResult Index()
        {
            var Price = db.T_RequestList.ToList();
            var data = db.T_Khruphanth.ToList();
            var TotalA = Price.Sum(a => a.RL_Amount);
            var TotalPrice = Price.Sum(x => x.RL_Price);
            TempData["ALO"] = data.Where(a => a.Kh_StatusID == 1).Count();
            TempData["ALI"] = data.Where(a => a.Kh_StatusID == 2).Count();
            TempData["ALX"] = TotalA * TotalPrice;
            return View();
        }
       
        public ActionResult ReportRequisition()
        {
            return View();

        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Details(string RequisitionID)
        {
            var data = db.T_Khruphanth.Where(x => x.KhruphanthID == RequisitionID).FirstOrDefault();
            return View(data);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Report()
        {
            return Redirect("~/Reports/ViewRequisition.aspx");
        }
        public ActionResult Report1()
        {
            return Redirect("~/Reports/QRCODE.aspx");
        }
        public ActionResult Report2()
        {
            return Redirect("~/Reports/Dis.aspx");
        }
        public ActionResult Login()
        {
            Session["CHKNAME"] = null;
            return View();
        }
        [HttpPost]
        public ActionResult Login(Account data)
        {
            var chk = db.Account.Where(a => a.AUsername == data.AUsername).FirstOrDefault();
            var chkpass = db.Account.Where(s => s.APassword == data.APassword).FirstOrDefault();
            if(chk == null)
            {
                ModelState.AddModelError("AUsername", "กรุณาตรวจสอบ username");
                return View(data);
            }
            else if(chkpass == null)
            {
                ModelState.AddModelError("APassword", "กรุณาตรวจสอบ username");
                return View(data);
            }
            else
            {
                Session["CHKNAME"] = 1;
                return RedirectToAction(nameof(Index));
            }
           
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            return RedirectToAction(nameof(Login));
        }
    }
}