using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApp.DAL;

namespace WebApp.Controllers
{
    public class CityController : Controller
    {
        protected ApplicationDbContext Db;
        public CityController()
        {
            Db = new ApplicationDbContext();
        }

        // GET: City/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: City/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name")] City city)
        {
            if (ModelState.IsValid)
            {
                Db.Cities.Add(city);
                Db.SaveChanges();
                return RedirectToAction("CustomerList", "Seller");
            }

            return View(city);
        }
    }
}