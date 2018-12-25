using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Khruphanth.Models;
namespace Khruphanth.Controllers
{
    public class KhruphanthsController : Controller
    {
        // GET: Khruphanths
        private ComCSDBEntities db = new ComCSDBEntities();
        public ActionResult Index()
        {
            var data = db.T_Khruphanth.ToList();
            return View(data);
        }

        // GET: Khruphanths/Details/5
        public ActionResult Details(string RequisitionID)
        {
            var data = db.T_Khruphanth.Where(x => x.KhruphanthID == RequisitionID).FirstOrDefault();
            return View(data);
        }

        // GET: Khruphanths/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Khruphanths/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Khruphanths/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Khruphanths/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Khruphanths/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Khruphanths/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
