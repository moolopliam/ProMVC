using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Khruphanth.Models;

namespace Khruphanth.Controllers
{
    public class TypeController : Controller
    {
        private readonly ComCSDBEntities db = new ComCSDBEntities();

        // GET: T_Category
        public ActionResult Index()
        {
            return View(db.T_Type.ToList());
        }

        // GET: T_Category/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_Type T_Type = db.T_Type.Find(id);
            if (T_Type == null)
            {
                return HttpNotFound();
            }
            return View(T_Type);
        }

        // GET: T_Category/Create
        public ActionResult Create()
        {
            ViewBag.TY_CategoryID = new SelectList(db.T_Category, "CategoryID", "CA_NameCategory");
            return View();
        }

        // POST: T_Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public ActionResult Create(T_Type T_Type)
        {
            ViewBag.TY_CategoryID = new SelectList(db.T_Category, "CategoryID", "CA_NameCategory");
            if (ModelState.IsValid)
            {
                var chk = db.T_Type.Where(c => c.TypeID == T_Type.TypeID).FirstOrDefault();
                if (chk == null)
                {

                    db.T_Type.Add(T_Type);
                    db.SaveChanges();
                    Session["Result"] = "okC";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("CategoryID", "มีหมวดนี้อยู่ในฐานข้อมูลแล้ว กรุณาตรวจสอบอีกครั้ง");
                }

            }

            return View(T_Type);
        }

        // GET: T_Category/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_Type T_Type = db.T_Type.Find(id);
            ViewBag.TY_CategoryID = new SelectList(db.T_Category, "CategoryID", "CA_NameCategory",T_Type.TypeID);
            if (T_Type == null)
            {
                return HttpNotFound();
            }
            return View(T_Type);
        }

        // POST: T_Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_Type T_Type)
        {
            ViewBag.TY_CategoryID = new SelectList(db.T_Category, "CategoryID", "CA_NameCategory", T_Type.TypeID);
            if (ModelState.IsValid)
            {
                db.Entry(T_Type).State = EntityState.Modified;
                db.SaveChanges();
                Session["Result"] = "okE";
                return RedirectToAction("Index");
            }
            return View(T_Type);
        }

        public ActionResult Delete(string id)
        {
            var data = db.T_Type.Where(a => a.TypeID.Contains(id)).FirstOrDefault();
            var chk = db.T_RequestList.Where(a => a.RL_TypeID.Contains(id)).FirstOrDefault();
            if (chk == null)
            {

                db.T_Type.Remove(data);
                db.SaveChanges();
                Session["Result"] = "ok";
                return RedirectToAction("Index");
            }
            Session["Result"] = "error";
            return RedirectToAction("Index");
        }
    }
}
