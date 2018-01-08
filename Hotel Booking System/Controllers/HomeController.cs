using Hotel_Booking_System.Controllers.ControllerExtensions;
using Hotel_Booking_System.Models;
using Hotel_Booking_System.Toast;
using Hotel_Booking_System.View_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Hotel_Booking_System.Controllers
{
    public class HomeController : MessageControllerBase
    {
        private BookingSystemModel db = new BookingSystemModel();

        public ActionResult Index()
        {
            ViewBag.RoomTypeId = new SelectList(db.RoomTypes, "id", "name", "Select room type");
            return View(new HomeDatePickerVM());
        }

        public ActionResult About()
        {
            ViewBag.Message = "Additional Info";

            var hotel = db.Hotels.First();

            IEnumerable<String> facilities = hotel.HotelFacilities.Select(v => v.Facility.name);
            return View(new AboutIndexVM
            {
                Facilities = facilities,
                Hotel = hotel
            });
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Contact";

            return View();
        }

        public ActionResult Search(DateTime startDate, DateTime endDate)
        {

            return View();
        }
    }
}