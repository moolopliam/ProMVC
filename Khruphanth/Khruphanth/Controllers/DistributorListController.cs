using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Khruphanth.Models;

namespace Khruphanth.Controllers
{
    public class DistributorListController : Controller
    {
        private ComCSDBEntities db = new ComCSDBEntities();
        // GET: Distributor
        public ActionResult Index()
        {
            var data = db.T_DistributorList.ToList();
            return View(data);
        }

        // GET: Distributor/Details/5
        public ActionResult Details(int id)
        {
            var data = db.T_DistributorList.Where(a => a.DistributorList == id).FirstOrDefault();
            return View(data);
        }

        // GET: Distributor/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Distributor/Create
        [HttpPost]
        public ActionResult Create(T_DistributorList data)
        {
            try
            {
                db.T_DistributorList.Add(data);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View(data);
            }
        }

        // GET: Distributor/Edit/5
        public ActionResult Edit(int id)
        {
            var data = db.T_DistributorList.Where(a => a.DistributorList == id).FirstOrDefault();
            return View(data);
        }

        // POST: Distributor/Edit/5
        [HttpPost]
        public ActionResult Edit(T_DistributorList data)
        {
            try
            {
                db.Entry(data).State = EntityState.Modified;
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }


        public ActionResult Delete(int id)
        {
            try
            {
                var data = db.T_DistributorList.Where(a => a.DistributorList == id).FirstOrDefault();
                db.T_DistributorList.Remove(data);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
