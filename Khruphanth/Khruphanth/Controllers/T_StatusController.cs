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
    public class T_StatusController : Controller
    {
        private ComCSDBEntities db = new ComCSDBEntities();

        // GET: T_Status
        public ActionResult Index()
        {
            return View(db.T_Status.ToList());
        }

        // GET: T_Status/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_Status t_Status = db.T_Status.Find(id);
            if (t_Status == null)
            {
                return HttpNotFound();
            }
            return View(t_Status);
        }

        // GET: T_Status/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: T_Status/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StatusID,Status_Name")] T_Status t_Status)
        {
            if (ModelState.IsValid)
            {
                db.T_Status.Add(t_Status);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(t_Status);
        }

        // GET: T_Status/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_Status t_Status = db.T_Status.Find(id);
            if (t_Status == null)
            {
                return HttpNotFound();
            }
            return View(t_Status);
        }

        // POST: T_Status/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StatusID,Status_Name")] T_Status t_Status)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_Status).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(t_Status);
        }

        // GET: T_Status/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_Status t_Status = db.T_Status.Find(id);
            if (t_Status == null)
            {
                return HttpNotFound();
            }
            return View(t_Status);
        }

        // POST: T_Status/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            T_Status t_Status = db.T_Status.Find(id);
            db.T_Status.Remove(t_Status);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
