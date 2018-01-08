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

namespace Hotel_Booking_System.Controllers
{
    public class RoomsController : Controller
    {
        private BookingSystemModel db = new BookingSystemModel();

        // GET: Rooms
        //public ActionResult Index()
        //{
        //    var rooms = db.Rooms.Where(v => !v.deleted).Include(r => r.Hotel).Include(r => r.HotelFloor).Include(r => r.RoomBand).Include(r => r.RoomPrice).Include(r => r.RoomType);
        //    return View(rooms.ToList());
        //}

        public ActionResult Index(DateTime? StartDate, DateTime? EndDate)
        {
            if ((StartDate != null && EndDate != null) || (Session[Globals.StartDateSessionVar] != null && Session[Globals.EndDateSessionVar] != null))
            {
                DateTime Start = StartDate != null ? (DateTime) StartDate : Convert.ToDateTime(Session[Globals.StartDateSessionVar]);
                DateTime End = EndDate != null ? (DateTime) EndDate : Convert.ToDateTime(Session[Globals.EndDateSessionVar]);

                Session[Globals.StartDateSessionVar] = Start;
                Session[Globals.EndDateSessionVar] = End;
                //ViewBag.StartDate = StartDate;
                //ViewBag.EndDate = EndDate;

                //List<Booking> bookings = db.Bookings.Where(v => ((Start >= v.startDate && Start <= v.endDate) || (End >= v.startDate && End <= v.endDate)) && !v.deleted).ToList();
                var allBookings = db.Bookings.Include(b => b.Customer).ToList();
                Booking first = allBookings.First();

                int compareStart = DateTime.Compare(Start, first.startDate);
                int compareEnd = DateTime.Compare(Start, first.endDate);


                List<Booking> bookings = db.Bookings.Where(v => ((DateTime.Compare(Start, v.startDate) >= 0 && DateTime.Compare(Start, v.endDate) <= 0)
                 && !v.deleted)).ToList();

                List<Room> invalidRooms = new List<Room>();

                foreach (Booking b in bookings)
                {
                    using (SqlConnection con = new SqlConnection("data source=localhost;initial catalog=HotelBookingSystem;integrated security=True;multipleactiveresultsets=True;application name=EntityFramework"))
                    {
                        using (SqlCommand cmd = new SqlCommand("getBookingRooms", con))
                        {
                            cmd.CommandType = CommandType.StoredProcedure;

                            cmd.Parameters.Add("@bookingId", SqlDbType.Int).Value = b.id;

                            con.Open();

                            using (SqlDataReader reader = cmd.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    invalidRooms.Add(new Room
                                    {
                                        id = Convert.ToInt32(reader[0]),
                                        hotel_id = Convert.ToInt32(reader[1]),
                                        hotelFloor_id = Convert.ToInt32(reader[2]),
                                        roomType_id = Convert.ToInt32(reader[3]),
                                        roomBand_id = Convert.ToInt32(reader[4]),
                                        roomPrice_id = Convert.ToInt32(reader[5]),
                                        additionalNotes = Convert.ToString(reader[6])
                                    });
                                }

                            }
                        }
                    }
                }

                List<Room> one = db.Rooms.Where(v => !v.deleted).ToList();
                List<Room> two = one.Where(v => invalidRooms.Where(vv => vv.id == v.id).FirstOrDefault() == null).ToList();

                return View(two);
            }
            else
            {
                return View(db.Rooms.Where(v => !v.deleted));
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
