using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using WebApp.DAL;
using WebApp.Models;

namespace WebApp.Controllers
{
    [Authorize(Roles = "Seller, Admin")]
    public class SellerController : Controller
    {
        protected ApplicationDbContext Db;
        protected ApplicationUserManager UserManager { get; set; }

        public SellerController()
        {
            Db = new ApplicationDbContext();
            UserManager = new ApplicationUserManager(new UserStore<ApplicationUser>(Db));
        }

        private IQueryable<Customer> GetCustomers()
        {
            var sellerId = User.Identity.GetUserId();
            var customers = Db.Customers.Include(c => c.CityRegion).Include(c => c.Classification);

            if (User.IsInRole("Seller"))
                customers = customers.Where(c => c.SellerID.Equals(sellerId));
            else if (User.IsInRole("Admin"))
                customers.Include(c => c.Seller);

            return customers;
        }

        // GET: Customers
        public ActionResult CustomerList()
        {
            var model = new CustomerListViewModel
            {
                Customers = GetCustomers().ToList()
            };
            ViewBag.CustomerClassificationID = new SelectList(Db.CustomerClassifications, "ID", "Name");
            ViewBag.CityID = new SelectList(Db.Cities, "ID", "Name");
            ViewBag.CityRegionID = new SelectList(Db.CityRegions, "ID", "Name");
            if (User.IsInRole("Admin")) ViewBag.SellerID = new SelectList(Db.Users, "ID", "Email");
            return View(model);
        }

        [HttpPost]
        public ActionResult SearchCustomer([Bind(Include = "Name,CityID,CityRegionID,CustomerClassificationID,Gender,LastPurchaseInitialDate,LastPurchaseFinalDate, SellerID")]FilterCustomerViewModel filters)
        {
            var customers = GetCustomers();

            if (!String.IsNullOrEmpty(filters.CustomerClassificationID))
                customers = customers.Where(c => c.Classification.ID.ToString().Equals(filters.CustomerClassificationID));

            if (!String.IsNullOrEmpty(filters.Name))
                customers = customers.Where(c => c.Name.Contains(filters.Name));

            if (!String.IsNullOrEmpty(filters.CityID))
                customers = customers.Where(c => c.CityRegion.City.ID.ToString().Equals(filters.CityID));

            if (!String.IsNullOrEmpty(filters.CityRegionID))
                customers = customers.Where(c => c.CityRegion.ID.ToString().Equals(filters.CityRegionID));

            if (!String.IsNullOrEmpty(filters.Gender))
            {
                var gender = Enum.Parse(typeof(Gender), filters.Gender);
                customers = customers.Where(c => c.Gender.ToString().Equals(gender.ToString()));
            }

            if (!String.IsNullOrEmpty(filters.LastPurchaseInitialDate))
            {
                var initialDate = DateTime.Parse(filters.LastPurchaseInitialDate);
                var finalDate = filters.LastPurchaseFinalDate.IsNullOrWhiteSpace() ? DateTime.Today : DateTime.Parse(filters.LastPurchaseFinalDate);
                customers = customers.Where(c => c.LastPurchase >= initialDate && c.LastPurchase <= finalDate);
            }

            if (!String.IsNullOrEmpty(filters.SellerID))
                customers = customers.Where(c => c.SellerID.ToString().Equals(filters.SellerID));

            var model = new CustomerListViewModel
            {
                Customers = customers.ToList(),
                Filters = filters
            };

            ViewBag.CustomerClassificationID = new SelectList(Db.CustomerClassifications, "ID", "Name");
            ViewBag.CityID = new SelectList(Db.Cities, "ID", "Name");
            ViewBag.CityRegionID = new SelectList(Db.CityRegions, "ID", "Name");
            if (User.IsInRole("Admin")) ViewBag.SellerID = new SelectList(Db.Users, "ID", "Email");

            return View("CustomerList", model);
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = GetCustomers().FirstOrDefault(c => c.ID.Equals((int)id));
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            ViewBag.CityRegionID = new SelectList(Db.CityRegions, "ID", "Name");
            ViewBag.CustomerClassificationID = new SelectList(Db.CustomerClassifications, "ID", "Name");
            ViewBag.CityID = new SelectList(Db.Cities, "ID", "Name");
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,PhoneNumber,CityRegionID,CustomerClassificationID,Gender,LastPurchase")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.SellerID = User.Identity.GetUserId();
                Db.Customers.Add(customer);
                Db.SaveChanges();
                return RedirectToAction("CustomerList");
            }

            ViewBag.CityRegionID = new SelectList(Db.CityRegions, "ID", "Name", customer.CityRegionID);
            ViewBag.CustomerClassificationID = new SelectList(Db.CustomerClassifications, "ID", "Name", customer.CustomerClassificationID);
            ViewBag.CityID = new SelectList(Db.Cities, "ID", "Name");
            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = GetCustomers().FirstOrDefault(c => c.ID.Equals((int)id));
            if (customer == null)
            {
                return HttpNotFound();
            }
            ViewBag.CityRegionID = new SelectList(Db.CityRegions, "ID", "Name", customer.CityRegionID);
            ViewBag.CustomerClassificationID = new SelectList(Db.CustomerClassifications, "ID", "Name", customer.CustomerClassificationID);
            ViewBag.CityID = new SelectList(Db.Cities, "ID", "Name");
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,PhoneNumber,CityRegionID,CustomerClassificationID,Gender,LastPurchase")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                customer.SellerID = Db.Customers.Find(customer.ID).SellerID;
                Db.Entry(customer).State = EntityState.Modified;
                Db.SaveChanges();
                return RedirectToAction("CustomerList");
            }
            ViewBag.CityRegionID = new SelectList(Db.CityRegions, "ID", "Name", customer.CityRegionID);
            ViewBag.CustomerClassificationID = new SelectList(Db.CustomerClassifications, "ID", "Name", customer.CustomerClassificationID);
            ViewBag.CityID = new SelectList(Db.Cities, "ID", "Name");
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = GetCustomers().FirstOrDefault(c => c.ID.Equals((int)id));
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Customer customer = GetCustomers().FirstOrDefault(c => c.ID.Equals(id));
            Db.Customers.Remove(customer);
            Db.SaveChanges();
            return RedirectToAction("CustomerList");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
