using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.DAL;

namespace WebApp.Controllers
{
    public class CityRegionController : Controller
    {
        protected ApplicationDbContext Db;
        public CityRegionController()
        {
            Db = new ApplicationDbContext();
        }

        // GET: City/Create
        public ActionResult Create()
        {
            ViewBag.CityID = new SelectList(Db.Cities, "ID", "Name");
            return View();
        }

        // POST: City/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,CityID")] CityRegion cityRegion)
        {
            if (ModelState.IsValid)
            {
                Db.CityRegions.Add(cityRegion);
                Db.SaveChanges();
                return RedirectToAction("CustomerList", "Seller");
            }
            ViewBag.CityID = new SelectList(Db.Cities, "ID", "Name");
            return View(cityRegion);
        }
    }
}