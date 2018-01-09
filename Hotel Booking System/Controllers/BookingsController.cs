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
using Hotel_Booking_System.Controllers.ControllerExtensions;
using Hotel_Booking_System.Toast;

namespace Hotel_Booking_System.Controllers
{
    public class BookingsController : MessageControllerBase
    {
        private BookingSystemModel db = new BookingSystemModel();

        // GET: Bookings
        public ActionResult Index(DateTime? StartDate, DateTime? EndDate, String DateFilter = null)
        {
            IQueryable<Booking> bookingsQuery = db.Bookings.Where(v => !v.deleted && !v.cancelled);
            ViewBag.DateFilter = new SelectList(Globals.BookingPeriods.Select(v => new SelectListItem { Value = v, Text = v }), "value", "value", "Select date filter");

            if (StartDate != null && StartDate != DateTime.MinValue && EndDate != null && EndDate != DateTime.MinValue && DateFilter != "")
            {
                DateTime Start = (DateTime) StartDate;
                DateTime End = (DateTime) EndDate;

                if (DateFilter == "Created On")
                    bookingsQuery = bookingsQuery.Where(v => (DateTime.Compare(Start, v.bookingMadeDate) <= 0 && DateTime.Compare(End, v.bookingMadeDate) >= 0));
                else if (DateFilter == "Start Date")
                    bookingsQuery = bookingsQuery.Where(v => (DateTime.Compare(Start, v.startDate) <= 0 && DateTime.Compare(End, v.startDate) >= 0));
                else
                    bookingsQuery = bookingsQuery.Where(v => (DateTime.Compare(Start, v.endDate) <= 0 && DateTime.Compare(End, v.endDate) >= 0));
            }

            var bookings = bookingsQuery.AsEnumerable().ToList();
            return View(new BookingIndexVM
            {
                Bookings = bookings.ToList().Select(v => new BookingVM
                {
                    ModelId = v.id,
                    Name = v.Customer.forename + " " + v.Customer.surname,
                    Created = v.bookingMadeDate.ToShortDateString() + " at " + v.bookingMadeTime.ToString(@"hh\:mm"),
                    From = v.startDate.ToShortDateString(),
                    To = v.endDate.ToShortDateString(),
                    NoRooms = v.RoomBookings.Count(),
                    Total = "£" + v.paymentTotal.ToString("0.00"),
                    PaymentMade = (v.Payments != null && v.Payments.Count() > 0) ? true : false,
                    IsGuest = v.guest_id != null
                })
            });
        }

        public ActionResult Search(DateTime? ChosenStartDate, DateTime? ChosenEndDate, String DateFilter = null)
        {
            if ((ChosenStartDate == null || ChosenStartDate == DateTime.MinValue) || (ChosenEndDate == null || ChosenEndDate == DateTime.MinValue) || DateFilter == "")
            {
                AddToastMessage("Search Issue", "Please enter valid properties for the filter", ToastType.Info);
                return RedirectToAction("Index");
            }
            else
                return RedirectToAction("Index", new { StartDate = ChosenStartDate, EndDate = ChosenEndDate, DateFilter = DateFilter });
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
                Created = booking.bookingMadeDate.ToLongDateString() + " at " + booking.bookingMadeTime.ToString(@"hh\:mm"),
                From = booking.startDate.ToLongDateString(),
                To = booking.endDate.ToLongDateString(),
                NoRooms = booking.RoomBookings.Count(),
                Total = "£" + booking.paymentTotal.ToString("0.00"),
                PaymentMade = (booking.Payments != null && booking.Payments.Count() > 0) ? true : false,
                PaymentDate = booking.paymentMadeDate != null ? ((DateTime)booking.paymentMadeDate).ToLongDateString() + " at " + ((DateTime)booking.paymentMadeDate).ToShortTimeString() : null,
                Rooms = rooms
            });
        }

        // GET: Bookings/Create
        public ActionResult Create()
        {
            List<int> roomIds = (List<int>)Session[Globals.CartSessionVar];

            if (roomIds == null || roomIds.Count() == 0)
            {
                var controller = (Request.UrlReferrer.Segments.Skip(1).Take(1).SingleOrDefault() ?? "Home").Trim('/');
                var action = (Request.UrlReferrer.Segments.Skip(2).Take(1).SingleOrDefault() ?? "Index").Trim('/');
                var id = (Request.UrlReferrer.Segments.Skip(3).Take(1).SingleOrDefault() ?? "").Trim('/');
                AddToastMessage("Empty Cart", "Please add a room to the cart", ToastType.Info);

                if (action == "Create" && controller == "Bookings")
                    return RedirectToAction("Index", "Home");
                else if (id != "")
                    return RedirectToAction(action, controller, new { id = id });
                else
                    return RedirectToAction(action, controller, HttpUtility.ParseQueryString(Request.UrlReferrer.Query)["SYSTEM"]);
            }

            ViewBag.customer_id = new SelectList(db.Customers, "id", "forename");
            ViewBag.start_date = Convert.ToDateTime(Session[Globals.StartDateSessionVar]).ToLongDateString();
            ViewBag.end_date = Convert.ToDateTime(Session[Globals.EndDateSessionVar]).ToLongDateString();

            List<Room> rooms = new List<Room>();
            double total = 0;

            foreach (int roomId in roomIds)
            {
                Room room = db.Rooms.Where(v => v.id == roomId).FirstOrDefault();
                rooms.Add(room);
                total += Convert.ToDouble(room.RoomPrice.price);
            }

            ViewBag.rooms = rooms;
            ViewBag.total = "£" + total.ToString("0.00");
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

            AddToastMessage("Cart Updated", "Room removed from the cart", ToastType.Success);
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

            List<int> roomIds = (List<int>) Session[Globals.CartSessionVar];
            double total = 0;

            foreach (int roomId in roomIds)
            {
                Room room = db.Rooms.Where(v => v.id == roomId).FirstOrDefault();
                total += Convert.ToDouble(room.RoomPrice.price);
            }

            booking.paymentTotal = Convert.ToDecimal(total);

            if (ModelState.IsValid)
            {
                db.Bookings.Add(booking);
                db.SaveChanges();

                foreach (int roomId in roomIds)
                {
                    db.RoomBookings.Add(new RoomBooking { booking_id = booking.id, room_id = roomId });
                }
                db.SaveChanges();

                Session[Globals.CartSessionVar] = null;

                return RedirectToAction("Create", "Payments", new { id = booking.id });
            }

            ViewBag.customer_id = new SelectList(db.Customers, "id", "forename", booking.customer_id);
            return View(booking);
        }

        // GET: Bookings/CheckIn/5
        public ActionResult CheckIn(int id)
        {
            Booking booking = db.Bookings.Find(id);

            List<String> phoneNos = new List<string>() { booking.Customer.homePhoneNo, booking.Customer.mobilePhoneNo };

            if (booking.Customer.workPhoneNo != null)
                phoneNos.Add(booking.Customer.workPhoneNo);

            ViewBag.ContactPhoneNoSelected = new SelectList(phoneNos.Select(v => new SelectListItem { Value = v, Text = v }), "value", "value");

            return View(new BookingCheckInVM
            {
                Booking = booking,
                BookingId = id
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CheckIn([Bind(Include = "Id,CustomerId,Title,Forename,Surname,DoB,AddressStreet,AddressTown,AddressCounty,AddressPostalCode,ContactPhoneNo,IsCustomerFromBooking,BookingId")] BookingCheckInVM bookingVM)
        {
            Booking booking = db.Bookings.Find(bookingVM.BookingId);

            Guest guest = bookingVM.IsCustomerFromBooking ? 
                new Guest {
                    customer_id = booking.Customer.id,
                    title = booking.Customer.title,
                    forename = booking.Customer.forename,
                    surname = booking.Customer.surname,
                    dob = booking.Customer.dob,
                    addressStreet = booking.Customer.addressStreet,
                    addressTown = booking.Customer.addressTown,
                    addressCounty = booking.Customer.addressCounty,
                    addressPostalCode = booking.Customer.addressPostalCode,
                    contactPhoneNo = bookingVM.ContactPhoneNoSelected
                } : 
                new Guest {
                    customer_id = null,
                    title = bookingVM.Title,
                    forename = bookingVM.Forename,
                    surname = bookingVM.Surname,
                    dob = bookingVM.DoB,
                    addressStreet = bookingVM.AddressStreet,
                    addressTown = bookingVM.AddressTown,
                    addressCounty = bookingVM.AddressCounty,
                    addressPostalCode = bookingVM.AddressPostalCode,
                    contactPhoneNo = bookingVM.ContactPhoneNo
                };

            if (ModelState.IsValid)
            {
                db.Guests.Add(guest);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return null;
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

        public ActionResult Cancel(int? id)
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

        // POST: Bookings/Cancel/5
        [HttpPost, ActionName("Cancel")]
        [ValidateAntiForgeryToken]
        public ActionResult CancelConfirmed(int id)
        {
            Booking booking = db.Bookings.Find(id);
            booking.cancelled = true;
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
