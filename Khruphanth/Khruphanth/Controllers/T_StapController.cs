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
    public class T_StapController : Controller
    {
        private ComCSDBEntities db = new ComCSDBEntities();

        // GET: T_Stap
        public ActionResult Index()
        {
            return View(db.T_Stap.ToList());
        }

        // GET: T_Stap/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_Stap t_Stap = db.T_Stap.Find(id);
            if (t_Stap == null)
            {
                return HttpNotFound();
            }
            return View(t_Stap);
        }

        // GET: T_Stap/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: T_Stap/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "StepID,ST_StepName")] T_Stap t_Stap)
        {
            if (ModelState.IsValid)
            {
                db.T_Stap.Add(t_Stap);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(t_Stap);
        }

        // GET: T_Stap/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_Stap t_Stap = db.T_Stap.Find(id);
            if (t_Stap == null)
            {
                return HttpNotFound();
            }
            return View(t_Stap);
        }

        // POST: T_Stap/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "StepID,ST_StepName")] T_Stap t_Stap)
        {
            if (ModelState.IsValid)
            {
                db.Entry(t_Stap).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(t_Stap);
        }

        // GET: T_Stap/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            T_Stap t_Stap = db.T_Stap.Find(id);
            if (t_Stap == null)
            {
                return HttpNotFound();
            }
            return View(t_Stap);
        }

        // POST: T_Stap/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            T_Stap t_Stap = db.T_Stap.Find(id);
            db.T_Stap.Remove(t_Stap);
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
