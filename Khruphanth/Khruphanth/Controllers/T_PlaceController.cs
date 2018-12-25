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
    public class T_PlaceController : Controller
    {
        private readonly ComCSDBEntities db = new ComCSDBEntities();

        // GET: T_Category
        public ActionResult Index()
        {
            return View(db.T_Place.ToList());
        }

        // GET: T_Category/Details/5
        public ActionResult Details(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var T_Place = db.T_Place.Where(a => a.PlaceID == id).FirstOrDefault();
            if (T_Place == null)
            {
                return HttpNotFound();
            }
            return View(T_Place);
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
        public ActionResult Create(T_Place data)
        {
            if (ModelState.IsValid)
            {
                var chk = db.T_Place.Where(c => c.PlaceID == data.PlaceID).FirstOrDefault();
                if (chk == null)
                {

                    db.T_Place.Add(data);
                    db.SaveChanges();
                    Session["Result"] = "okC";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("CategoryID", "มีหมวดนี้อยู่ในฐานข้อมูลแล้ว กรุณาตรวจสอบอีกครั้ง");
                }

            }

            return View(data);
        }

        // GET: T_Category/Edit/5
        public ActionResult Edit(int id)
        {
            if (id == 0)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var T_Place = db.T_Place.Where(a => a.PlaceID == id).FirstOrDefault();
            if (T_Place == null)
            {
                return HttpNotFound();
            }
            return View(T_Place);
        }

        // POST: T_Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(T_Place T_Place)
        {
            if (ModelState.IsValid)
            {
                db.Entry(T_Place).State = EntityState.Modified;
                db.SaveChanges();
                Session["Result"] = "okE";
                return RedirectToAction("Index");
            }
            return View(T_Place);
        }

        public ActionResult Delete(int PlaceID)
        {
            var data = db.T_Place.Where(a => a.PlaceID == PlaceID).FirstOrDefault();
            var chk = db.T_Khruphanth.Where(a => a.Kh_PlaceID == PlaceID).FirstOrDefault();
            if (chk == null)
            {

                db.T_Place.Remove(data);
                db.SaveChanges();
                Session["Result"] = "ok";
                return RedirectToAction("Index");
            }
            Session["Result"] = "error";
            return RedirectToAction("Index");
        }
    }
}
