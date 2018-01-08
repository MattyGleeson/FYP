using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hotel_Booking_System.Models;
using Hotel_Booking_System.Global;

namespace Hotel_Booking_System.Controllers
{
    public class PaymentsController : Controller
    {
        private BookingSystemModel db = new BookingSystemModel();

        // GET: Payments
        public ActionResult Index()
        {
            var payments = db.Payments.Include(p => p.Booking).Include(p => p.Customer).Include(p => p.PaymentMethod);
            return View(payments.ToList());
        }

        // GET: Payments/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // GET: Payments/Create
        public ActionResult Create()
        {
            Booking booking = (Booking) Session[Globals.BookingSessionVar];

            ViewBag.booking_id = new SelectList(db.Bookings, "id", "comments");
            ViewBag.customer_id = new SelectList(db.Customers, "id", "forename");
            ViewBag.paymentMethod_id = new SelectList(db.PaymentMethods, "id", "name");
            ViewBag.total = booking.paymentTotal;
            ViewBag.customer = booking.Customer.forename + " " + booking.Customer.surname;

            return View();
        }

        // POST: Payments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,paymentMethod_id,customer_id,booking_id,amount,comments,deleted")] Payment payment)
        {
            Booking booking = (Booking)Session[Globals.BookingSessionVar];

            payment.booking_id = 1;
            payment.amount = booking.paymentTotal;
            payment.customer_id = booking.customer_id;
            payment.Customer = booking.Customer;

            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();

                List<int> roomIds = (List<int>)Session[Globals.CartSessionVar];
                foreach (int roomId in roomIds)
                {
                    db.RoomBookings.Add(new RoomBooking { booking_id = booking.id, room_id = roomId });
                }
                db.SaveChanges();

                payment.booking_id = booking.id;
                db.Payments.Add(payment);
                db.SaveChanges();

                Session[Globals.CartSessionVar] = null;
                Session[Globals.BookingSessionVar] = null;
                Session[Globals.StartDateSessionVar] = null;
                Session[Globals.EndDateSessionVar] = null;

                return RedirectToAction("Index", "Bookings");
            }

            ViewBag.booking_id = new SelectList(db.Bookings, "id", "comments", payment.booking_id);
            ViewBag.customer_id = new SelectList(db.Customers, "id", "forename", payment.customer_id);
            ViewBag.paymentMethod_id = new SelectList(db.PaymentMethods, "id", "name", payment.paymentMethod_id);
            return View(payment);
        }

        // GET: Payments/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            ViewBag.booking_id = new SelectList(db.Bookings, "id", "comments", payment.booking_id);
            ViewBag.customer_id = new SelectList(db.Customers, "id", "forename", payment.customer_id);
            ViewBag.paymentMethod_id = new SelectList(db.PaymentMethods, "id", "name", payment.paymentMethod_id);
            return View(payment);
        }

        // POST: Payments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,paymentMethod_id,customer_id,booking_id,amount,comments,deleted")] Payment payment)
        {
            if (ModelState.IsValid)
            {
                db.Entry(payment).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.booking_id = new SelectList(db.Bookings, "id", "comments", payment.booking_id);
            ViewBag.customer_id = new SelectList(db.Customers, "id", "forename", payment.customer_id);
            ViewBag.paymentMethod_id = new SelectList(db.PaymentMethods, "id", "name", payment.paymentMethod_id);
            return View(payment);
        }

        // GET: Payments/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Payment payment = db.Payments.Find(id);
            if (payment == null)
            {
                return HttpNotFound();
            }
            return View(payment);
        }

        // POST: Payments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Payment payment = db.Payments.Find(id);
            db.Payments.Remove(payment);
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
