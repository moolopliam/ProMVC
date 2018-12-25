using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Khruphanth.Inheritance;
using Khruphanth.Models;

namespace Khruphanth.Controllers
{
    public class DistributorController : Controller
    {
        private readonly ComCSDBEntities db = new ComCSDBEntities();

        // GET: T_Category
        public ActionResult Index()
        {
            var data = db.T_Distributor.ToList();
            return View(data);
        }
        public ActionResult IndexR()
        {
            var data = db.T_Distributor.ToList();
            var a = db.T_Distributor.Where(i => i.Di_Status == "3").ToList();
            TempData["a"] = a.Count;
            return View(data);
        }

        // GET: T_Category/Details/5
        public ActionResult Details(string DistributorID)
        {
            if (DistributorID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Session["CHK"] = 1;
            var data = db.T_DistributorList.Where(a => a.DL_DistributorID == DistributorID).ToList();
            if (data.Count > 0)
            {
                Session["CHK"] = 0;
            }
            Session["IDD"] = DistributorID;
            return View(data);
        }
        public ActionResult DetailsR(string DistributorID)
        {


            var data = db.T_DistributorList.Where(a => a.DL_DistributorID == DistributorID).ToList();

            return View(data);
        }
        public ActionResult Disbu(string DL)
        {
            if (DL != null)
            {
                var data = db.T_Distributor.ToList();
                var Ex = data.Where(a => a.DistributorID == DL).FirstOrDefault();
                Ex.Di_Status = "2";
                db.Entry(Ex).State = EntityState.Modified;
                db.SaveChanges();
                Chang(DL);
                return RedirectToAction(nameof(IndexR));
            }
            else
            {
                Session["Result"] = "error8";
                return RedirectToAction(nameof(IndexR));
            }
        }
        public void Chang(string ID)
        {
            var data = db.T_DistributorList.Where(a => a.DL_DistributorID == ID).ToList();
            foreach (var item in data)
            {
                var KP = db.T_Khruphanth.Where(a => a.KhruphanthID == item.DL_KhruphanthID).FirstOrDefault();
                KP.Kh_StatusID = 2;
                db.Entry(KP).State = EntityState.Modified;
            }
            db.SaveChanges();

        }


        public ActionResult DetailKhruphat(string KpID)
        {
            var result = db.T_Khruphanth.Where(a => a.KhruphanthID == KpID).FirstOrDefault();
            return View(result);
        }
        public ActionResult Khruphat()
        {
            var data = db.T_Khruphanth.Where(a => a.Kh_StatusID == 1).ToList();
            return View(data);
        }

        public ActionResult leave(string Khruphat)
        {
            var Kr = db.T_Khruphanth.Where(a => a.KhruphanthID == Khruphat).FirstOrDefault();

            var tmp = Session["IDD"].ToString();
            T_DistributorList DList = new T_DistributorList()
            {
                DL_DistributorID = tmp,
                DL_KhruphanthID = Kr.KhruphanthID
            };
            db.T_DistributorList.Add(DList);
            Kr.Kh_StatusID = 3;
            db.Entry(Kr).State = EntityState.Modified;
            db.SaveChanges();
            Session["Result"] = "okC";
            Session.Remove("IDD");
            return RedirectToAction("Details", new { DistributorID = tmp });
        }

        // GET: T_Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: T_Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(T_Distributor data)
        {
            if (ModelState.IsValid)
            {
                if (Convert.ToInt32(data.DistributorID) < 0)
                {
                    ModelState.AddModelError("DistributorID", "กรุณาตรวจสอบ กรุณากรอกอีกครั้ง");
                    return View(data);
                }
                int result;
                var p = int.TryParse(data.DistributorID, out result);
                if (p == true)
                {
                    var Year = DateTime.Now.ToString("yy");
                    data.DistributorID = data.DistributorID + "/" + Year;
                    var chk = db.T_Distributor.Where(c => c.DistributorID == data.DistributorID).FirstOrDefault();
                    if (chk == null)
                    {

                        data.Di_Status = "3";
                        db.T_Distributor.Add(data);
                        db.SaveChanges();
                        Session["Result"] = "okC";
                        return RedirectToAction("IndexR");
                    }
                    else
                    {
                        ModelState.AddModelError("DistributorID",
                            "มีข้อมูลนี้อยู่ในฐานข้อมูลแล้ว กรุณาตรวจสอบอีกครั้ง");
                        return View(data);
                    }
                }
                else
                {                   
                    ModelState.AddModelError("DistributorID", "กรุณากรอกเลขที่ใบจำหน่ายเป็นตัวเลข");
                    return View(data);
                }

            }

            return View(data);
        }

        // GET: T_Category/Edit/5
        public ActionResult Edit(string DistributorID)
        {

            var t_Category = db.T_Distributor.Where(a => a.DistributorID == DistributorID).FirstOrDefault();
            return View(t_Category);
        }

        // POST: T_Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]

        public ActionResult Edit(T_Distributor data)
        {
            if (ModelState.IsValid)
            {
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                Session["Result"] = "okE";
                return RedirectToAction("Index");
            }
            return View(data);
        }

        public ActionResult Delete(string DistributorID)
        {
            var data = db.T_Distributor.Where(a => a.DistributorID == DistributorID).FirstOrDefault();
            var chk = db.T_DistributorList.Where(a => a.DL_DistributorID == data.DistributorID).ToList();
            try
            {
                if (chk.Count == 0)
                {

                    db.T_Distributor.Remove(data);
                    db.SaveChanges();
                    Session["Result"] = "ok";
                    return RedirectToAction("IndexR");
                }
                else
                {
                    db.T_Distributor.Remove(data);
                    db.T_DistributorList.RemoveRange(chk);
                    db.SaveChanges();
                    Session["Result"] = "ok";
                    return RedirectToAction("IndexR");
                }
            }
            catch (Exception )
            {

                Session["Result"] = "error";
                return RedirectToAction("IndexR");
            }

        }

    }
}
