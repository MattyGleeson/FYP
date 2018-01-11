using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hotel_Booking_System.Models;
using Hotel_Booking_System.Controllers.ControllerExtensions;
using Hotel_Booking_System.View_Models;

namespace Hotel_Booking_System.Controllers
{
    public class CustomersController : MessageControllerBase
    {
        private BookingSystemModel db = new BookingSystemModel();

        // GET: Customers
        public ActionResult Index()
        {
            return View(db.Customers.ToList());
        }

        // GET: Customers/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // GET: Customers/Create
        public ActionResult Create()
        {
            return View(new CreateCustomerVM());
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Title,Forename,Surname,DoB,HouseNo,Street,Town,County,PostCode,HomePhoneNo,WorkPhoneNo,MobPhoneNo,Email")] CreateCustomerVM customer)
        {
            if (ModelState.IsValid)
            {
                db.Customers.Add(new Customer
                {
                    title = customer.Title,
                    forename = customer.Forename,
                    surname = customer.Surname,
                    dob = customer.DoB,
                    addressStreet = customer.HouseNo + ", " + customer.Street,
                    addressTown = customer.Town,
                    addressCounty = customer.County,
                    addressPostalCode = customer.PostCode,
                    homePhoneNo = customer.HomePhoneNo,
                    workPhoneNo = customer.WorkPhoneNo,
                    mobilePhoneNo = customer.MobPhoneNo,
                    email = customer.Email
                });
                db.SaveChanges();
                return RedirectToAction("Index","Home");
            }

            return View(customer);
        }

        // GET: Customers/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
            if (customer == null)
            {
                return HttpNotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,title,forename,surname,dob,addressStreet,addressTown,addressCounty,addressPostalCode,homePhoneNo,workPhoneNo,mobilePhoneNo,email,deleted")] Customer customer)
        {
            if (ModelState.IsValid)
            {
                db.Entry(customer).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(customer);
        }

        // GET: Customers/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Customer customer = db.Customers.Find(id);
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
            Customer customer = db.Customers.Find(id);
            customer.deleted = true;
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
