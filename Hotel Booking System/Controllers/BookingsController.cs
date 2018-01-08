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
using Hotel_Booking_System.View_Models;

namespace Hotel_Booking_System.Controllers
{
    public class BookingsController : Controller
    {
        private BookingSystemModel db = new BookingSystemModel();

        // GET: Bookings
        public ActionResult Index()
        {
            var bookings = db.Bookings.Where(v => !v.deleted).Include(b => b.Customer);
            return View(bookings.ToList().Select(v => new BookingIndexVM
            {
                ModelId = v.id,
                Name = v.Customer.forename + " " + v.Customer.surname,
                Created = v.bookingMadeDate.ToShortDateString() + " at " + v.bookingMadeTime.ToString(@"hh\:mm"),
                From = v.startDate.ToShortDateString(),
                To = v.endDate.ToShortDateString(),
                NoRooms = v.RoomBookings.Count(),
                Total = "£" + v.paymentTotal.ToString("0.00"),
                PaymentMade = (v.Payments != null && v.Payments.Count() > 0) ? true : false,
            }));
        }

        // GET: Bookings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }

            List<Room> rooms = StoredProcedures.GetBookingRooms(booking.id).ToList();

            return View(new BookingDetailsVM
            {
                ModelId = booking.id,
                Name = booking.Customer.forename + " " + booking.Customer.surname,
                Created = booking.bookingMadeDate.ToShortDateString() + " at " + booking.bookingMadeTime.ToString(@"hh\:mm"),
                From = booking.startDate.ToShortDateString(),
                To = booking.endDate.ToShortDateString(),
                NoRooms = booking.RoomBookings.Count(),
                Total = "£" + booking.paymentTotal.ToString("0.00"),
                PaymentMade = (booking.Payments != null && booking.Payments.Count() > 0) ? true : false,
                Rooms = rooms
            });
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            List<int> roomIds = (List<int>)Session[Globals.CartSessionVar];

            if (roomIds == null || roomIds.Count() == 0)
                return Redirect(Request.UrlReferrer.ToString());

            ViewBag.customer_id = new SelectList(db.Customers, "id", "forename");
            ViewBag.start_date = Session[Globals.StartDateSessionVar];
            ViewBag.end_date = Session[Globals.EndDateSessionVar];
            
            List<Room> rooms = new List<Room>();
            double total = 0;

            foreach (int roomId in roomIds)
            {
                Room room = db.Rooms.Where(v => v.id == roomId).FirstOrDefault();
                rooms.Add(room);
                total += Convert.ToDouble(room.RoomPrice.price);
            }

            ViewBag.rooms = rooms;
            ViewBag.total = total;
            return View();
        }

        public ActionResult RemoveRoomFromCart(int id)
        {
            List<int> roomIds = (List<int>) Session[Globals.CartSessionVar];

            for (int i = 0; i < roomIds.Count(); i++)
            {
                if (roomIds[i] == id)
                    roomIds.RemoveAt(i);
            }

            Session[Globals.CartSessionVar] = roomIds;

            return RedirectToAction("Create");
        }

        // POST: Bookings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,customer_id,bookingMadeDate,bookingMadeTime,startDate,endDate,paymentTotal,paymentDueDate,paymentMadeDate,comments,deleted")] Booking booking)
        {
            booking.startDate = Convert.ToDateTime(Session[Globals.StartDateSessionVar]);
            booking.endDate = Convert.ToDateTime(Session[Globals.EndDateSessionVar]);
            booking.paymentDueDate = Convert.ToDateTime(Session[Globals.StartDateSessionVar]);
            booking.bookingMadeDate = DateTime.Now.Date;
            booking.bookingMadeTime = DateTime.Now.TimeOfDay;
            booking.Customer = db.Customers.Where(v => v.id == booking.customer_id).FirstOrDefault();

            List<int> roomIds = (List<int>)Session[Globals.CartSessionVar];
            double total = 0;

            foreach (int roomId in roomIds)
            {
                Room room = db.Rooms.Where(v => v.id == roomId).FirstOrDefault();
                total += Convert.ToDouble(room.RoomPrice.price);
            }

            booking.paymentTotal = Convert.ToDecimal(total);

            if (ModelState.IsValid)
            {
                Session[Globals.BookingSessionVar] = booking;
                //db.Bookings.Add(booking);
                //db.SaveChanges();
                return RedirectToAction("Create", "Payments");
            }

            ViewBag.customer_id = new SelectList(db.Customers, "id", "forename", booking.customer_id);
            return View(booking);
        }

        // GET: Bookings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            ViewBag.customer_id = new SelectList(db.Customers, "id", "forename", booking.customer_id);
            return View(booking);
        }

        // POST: Bookings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,customer_id,bookingMadeDate,bookingMadeTime,startDate,endDate,paymentTotal,paymentDueDate,paymentMadeDate,comments,deleted")] Booking booking)
        {
            if (ModelState.IsValid)
            {
                db.Entry(booking).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.customer_id = new SelectList(db.Customers, "id", "forename", booking.customer_id);
            return View(booking);
        }

        // GET: Bookings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Booking booking = db.Bookings.Find(id);
            if (booking == null)
            {
                return HttpNotFound();
            }
            return View(new BookingDetailsVM
            {
                ModelId = booking.id,
                Name = booking.Customer.forename + " " + booking.Customer.surname,
                Created = booking.bookingMadeDate.ToShortDateString() + " at " + booking.bookingMadeTime.ToString(@"hh\:mm"),
                From = booking.startDate.ToShortDateString(),
                To = booking.endDate.ToShortDateString(),
                NoRooms = booking.RoomBookings.Count(),
                Total = "£" + booking.paymentTotal.ToString("0.00"),
                PaymentMade = (booking.Payments != null && booking.Payments.Count() > 0) ? true : false,
                Rooms = StoredProcedures.GetBookingRooms(booking.id)
            });
        }

        // POST: Bookings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            if (StoredProcedures.DeleteBooking(id, db.Bookings.Find(id).Customer.id))
                return RedirectToAction("Index");
            else
                return null;
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
