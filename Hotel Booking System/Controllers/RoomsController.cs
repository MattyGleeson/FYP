using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hotel_Booking_System.Models;
using Hotel_Booking_System.View_Models;
using System.Data.SqlClient;
using System.Web.Caching;
using Hotel_Booking_System.Global;
using Hotel_Booking_System.Controllers.ControllerExtensions;
using Hotel_Booking_System.Toast;

namespace Hotel_Booking_System.Controllers
{
    public class RoomsController : MessageControllerBase
    {
        private BookingSystemModel db = new BookingSystemModel();

        // GET: Rooms
        //public ActionResult Index()
        //{
        //    var rooms = db.Rooms.Where(v => !v.deleted).Include(r => r.Hotel).Include(r => r.HotelFloor).Include(r => r.RoomBand).Include(r => r.RoomPrice).Include(r => r.RoomType);
        //    return View(rooms.ToList());
        //}

        public ActionResult Index(DateTime? StartDate, DateTime? EndDate, int? RoomTypeId)
        {
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "id", "name", "Select room type");
            if ((StartDate != null && StartDate != DateTime.MinValue && EndDate != null && EndDate != DateTime.MinValue) || (Session[Globals.StartDateSessionVar] != null && Session[Globals.EndDateSessionVar] != null))
            {
                DateTime Start = StartDate != null ? (DateTime) StartDate : Convert.ToDateTime(Session[Globals.StartDateSessionVar]);
                DateTime End = EndDate != null ? (DateTime) EndDate : Convert.ToDateTime(Session[Globals.EndDateSessionVar]);

                Session[Globals.StartDateSessionVar] = Start;
                Session[Globals.EndDateSessionVar] = End;

                IQueryable<Booking> bookingsQuery = db.Bookings.Where(v => !v.deleted);

                bookingsQuery.Where(v => (DateTime.Compare(Start, v.startDate) >= 0 && DateTime.Compare(Start, v.endDate) <= 0));
                bookingsQuery.Where(v => (DateTime.Compare(End, v.startDate) >= 0 && DateTime.Compare(End, v.endDate) <= 0));
                bookingsQuery.Where(v => (DateTime.Compare(Start, v.startDate) <= 0 && DateTime.Compare(End, v.endDate) >= 0));

                List<Booking> bookings = bookingsQuery.ToList();
                List<Room> invalidRooms = new List<Room>();

                foreach (Booking b in bookings)
                {
                    invalidRooms.AddRange(StoredProcedures.GetBookingRooms(b.id));
                }

                IQueryable<Room> allRooms = db.Rooms.Where(v => !v.deleted);
                if (RoomTypeId != null)
                    allRooms = allRooms.Where(v => v.roomType_id == RoomTypeId);

                List<Room> validRooms = allRooms.ToList().Where(v => invalidRooms.Where(vv => vv.id == v.id).FirstOrDefault() == null).ToList();

                return View(new RoomIndexVM { Rooms = validRooms });
            }
            else
            {
                if (((StartDate != null && StartDate != DateTime.MinValue) || (Session[Globals.StartDateSessionVar] != null)) &&
                    (EndDate == null || EndDate == DateTime.MinValue || Session[Globals.EndDateSessionVar] == null))
                {
                    var controller = (Request.UrlReferrer.Segments.Skip(1).Take(1).SingleOrDefault() ?? "Home").Trim('/');
                    var action = (Request.UrlReferrer.Segments.Skip(2).Take(1).SingleOrDefault() ?? "Index").Trim('/');
                    AddToastMessage("Search Error Occured", "Please select an end date", ToastType.Error);
                    return RedirectToAction(action, controller);
                }
                else if (((EndDate != null && EndDate != DateTime.MinValue) || (Session[Globals.EndDateSessionVar] != null)) &&
                    (StartDate == null || StartDate == DateTime.MinValue || Session[Globals.StartDateSessionVar] == null))
                {
                    var controller = (Request.UrlReferrer.Segments.Skip(1).Take(1).SingleOrDefault() ?? "Home").Trim('/');
                    var action = (Request.UrlReferrer.Segments.Skip(2).Take(1).SingleOrDefault() ?? "Index").Trim('/');
                    AddToastMessage("Search Error Occured", "Please select a start date", ToastType.Error);
                    return RedirectToAction(action, controller);
                }
                else
                {
                    IQueryable<Room> allRooms = db.Rooms.Where(v => !v.deleted);
                    if (RoomTypeId != null)
                        allRooms = allRooms.Where(v => v.roomType_id == RoomTypeId);
                    return View(new RoomIndexVM { Rooms = allRooms.ToList() });
                }
            }
        }

        // GET: Rooms/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = db.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }

            IEnumerable<String> facilities = room.RoomFacilities.Select(v => v.Facility.name);
            
            return View(new RoomDetailsVM
            {
                Room = room,
                Facilities = string.Join(", ", facilities)
            });
        }

        public ActionResult AddToCart(int? id)
        {
            if (Session[Globals.CartSessionVar] != null)
            {
                List<int> roomIds = (List<int>)(Session[Globals.CartSessionVar]);

                roomIds.Add((int) id);
            }
            else
            {
                Session[Globals.CartSessionVar] = new List<int> { (int) id };
            }
            AddToastMessage("Cart Updated", "The room has been added to the cart", ToastType.Success);
            return RedirectToAction("Index", new { StartDate = Convert.ToDateTime(Session[Globals.StartDateSessionVar]), EndDate = Convert.ToDateTime(Session[Globals.EndDateSessionVar]) });
        }

        private IEnumerable<Booking> getBookingsForRooms(IEnumerable<Room> rooms)
        {
            HashSet<Booking> res = new HashSet<Booking>();
            List<Booking> bookings = db.Bookings.ToList();
            List<RoomBooking> roomBookings = bookings.SelectMany(v => v.RoomBookings.ToList()).ToList();

            foreach (RoomBooking rb in roomBookings)
            {
                if (rooms.Contains(rb.Room))
                    res.Add(rb.Booking);
            }

            return res;
        }
    }
}
