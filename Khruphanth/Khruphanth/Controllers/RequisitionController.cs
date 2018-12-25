using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Khruphanth.Models;

namespace Khruphanth.Controllers
{
    public class RequisitionController : Controller
    {

        // GET: Requisition
        private readonly ComCSDBEntities db = new ComCSDBEntities();

        public ActionResult Waiting()
        {

            var data = db.T_Requisition.Where(x => x.Re_StepID == "0").ToList();
            var i = 0;
            foreach (var item in data)
            {
                i++;
            }
            TempData["amout"] = i;
            return View(GetT_Requisitions());
        }
        public List<T_Requisition> GetT_Requisitions()
        {
            var data = db.T_Requisition.Where(x => x.Re_StepID == "0").ToList();
            var i = 0;
            foreach (var item in data)
            {
                i++;
            }
            TempData["amout"] = i;
            return db.T_Requisition.ToList();
        }




        public ActionResult Index()
        {
            return View(db.T_Requisition.ToList());
        }

        // GET: Requisition/Details/5

        public ActionResult Details(string RequisitionID)
        {
            if (RequisitionID != null)
            {
                Session["RequisitionID"] = RequisitionID;

            }
            else
            {
                return RedirectToAction(nameof(Waiting));
            }

            var data = db.T_RequestList.Where(x => x.RL_RequisitionID == RequisitionID).ToList();
            Session["IX"] = 0;
            if (data.Count > 0)
            {
                Session["IX"] = 1;
            }
            return View(data);
        }

        public ActionResult DetailsS(string RequisitionID)
        {

            if (RequisitionID != null)
            {
                Session["RequisitionID"] = RequisitionID;

            }
            else
            {
                return RedirectToAction(nameof(Waiting));
            }
            var data = db.T_RequestList.Where(x => x.RL_RequisitionID == RequisitionID).ToList();

            return View(data);
        }

        // GET: Requisition/Create
        public ActionResult Create()
        {
            var data = db.Teacher.ToList();
            var value = new List<TmpTeacher>();
            foreach (var item in data)
            {
                value.Add(new TmpTeacher { IDT = item.TeaId, NAMEFULL = item.Title.TName + "     " + item.TeaName });
            }
            ViewBag.Re_TeaId = new SelectList(value, "IDT", "NAMEFULL");

            return View();
        }

        // POST: Requisition/Create
        [HttpPost]
        public ActionResult Create(T_Requisition data)
        {
            var data1 = db.Teacher.ToList();

            var value = new List<TmpTeacher>();
            foreach (var item in data1)
            {
                value.Add(new TmpTeacher { IDT = item.TeaId, NAMEFULL = item.Title.TName + "     " + item.TeaName });
            }
            ViewBag.Re_TeaId = new SelectList(value, "IDT", "NAMEFULL");
            if (Convert.ToInt32(data.RequisitionID) < 0)
            {
                ModelState.AddModelError("RequisitionID", "กรุณาตรวจสอบ กรุณากรอกอีกครั้ง");
                return View(data);
            }
            var Chk = db.T_Requisition.Where(x => x.RequisitionID == data.RequisitionID).FirstOrDefault();
            if (ModelState.IsValid)
            {
                try
                {
                    int result;
                    var p = int.TryParse(data.RequisitionID, out result);
                    if (p == true)
                    {
                        var Year = DateTime.Now.ToString("yy");
                        data.RequisitionID = data.RequisitionID + "/" + Year;
                        if (Chk == null)
                        {
                            data.Re_StepID = "0";
                            db.T_Requisition.Add(data);
                            db.SaveChanges();
                            Session["Result"] = "okC";
                            Session["RequisitionID"] = data.RequisitionID;
                            return RedirectToAction("Waiting", "Requisition");
                        }
                        else
                        {

                            ViewBag.Re_TeaId = new SelectList(value, "IDT", "NAMEFULL");
                            return View(data);
                        }
                    }
                    else
                    {

                        ModelState.AddModelError("RequisitionID", "กรุณากรอกเลขที่ใบเบิกเป็นตัวเลข");
                        return View(data);
                    }
                }
                catch
                {

                    return View(data);
                }
            }
            else
            {

                return View(data);
            }

        }

        public JsonResult GetProductsByCategoryId(string id = "0")
        {
            var ID = Int32.Parse(id);

            List<T_Type> Type = new List<T_Type>();
            if (ID != 0)
            {
                Type = db.T_Type.Where(p => p.TY_CategoryID == id).ToList();
                //Type.Insert(0, new T_Type { TypeID = 0, NameType = "กรุณาเลือกหมวดหมู่" });

            }
            else
            {
                Type.Insert(0, new T_Type { TypeID = "0", TY_NameType = "กรุณาเลือกหมวดหมู่" });
            }
            var result = (from r in Type
                          select new
                          {
                              id = r.TypeID,
                              name = r.TY_NameType
                          }).ToList();

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        // GET: Requisition/Edit/5
        public ActionResult Edit(string RequisitionID)
        {

            ViewBag.Re_TeaId = new SelectList(db.Teacher, "TeaId", "TeaName");
            if (RequisitionID == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            return View(db.T_Requisition.Where(a => a.RequisitionID == RequisitionID).FirstOrDefault());
        }

        // POST: Requisition/Edit/5
        [HttpPost]
        public ActionResult Edit(T_Requisition data)
        {
            ViewBag.Re_TeaId = new SelectList(db.Teacher, "TeaId", "TeaName");
            try
            {
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();
                Session["Result"] = "okE";
                return RedirectToAction(nameof(Waiting));
            }
            catch
            {
                return View(data);
            }
        }

        // GET: Requisition/Delete/5

        public ActionResult Delete(string RequisitionID = null)
        {

            if (RequisitionID == null)
            {
                Session["Result"] = "error";
                return View(nameof(Waiting));
            }
            var CHKDATA = db.T_Requisition.Where(a => a.RequisitionID == RequisitionID).FirstOrDefault();
            if (CHKDATA != null)
            {

                var RQLIST = db.T_Requisition.Where(a => a.RequisitionID == RequisitionID).FirstOrDefault();

                var LLIST = db.T_RequestList.Where(x => x.RL_RequisitionID == RQLIST.RequisitionID).ToList();
                if (LLIST.Count == 0)
                {
                    db.T_Requisition.Remove(RQLIST);
                    db.SaveChanges();
                    Session["Result"] = "ok";
                    return RedirectToAction(nameof(Waiting));
                }
                else
                {
                    var CHK = db.T_RequestList.Where(x => x.RL_RequisitionID == RequisitionID).FirstOrDefault();
                    var Khruphanth = db.T_Khruphanth.Where(v => v.Kh_RequestLsitID == CHK.RequestLsitID).ToList();
                    db.T_Khruphanth.RemoveRange(Khruphanth);
                    db.T_RequestList.RemoveRange(LLIST);
                    db.SaveChanges();
                    Session["Result"] = "ok";
                    return RedirectToAction(nameof(Waiting));
                }

            }
            else
            {
                Session["Result"] = "error";
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //return View("Waiting"+GetT_Requisitions());
        }


    }
}
