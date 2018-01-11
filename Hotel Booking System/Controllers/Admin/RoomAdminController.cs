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

namespace Hotel_Booking_System.Controllers.Admin
{
    public class RoomAdminController : MessageControllerBase
    {
        private BookingSystemModel db = new BookingSystemModel();

        // GET: Rooms
        public ActionResult Index()
        {
            var rooms = db.Rooms.Where(v => !v.deleted).Include(r => r.Hotel).Include(r => r.HotelFloor).Include(r => r.RoomBand).Include(r => r.RoomPrice).Include(r => r.RoomType);
            return View(rooms.ToList());
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
            IEnumerable<Facility> roomFacilities = room.RoomFacilities.Where(v => !v.deleted).Select(v => v.Facility);

            ViewBag.facilities = roomFacilities;

            return View(room);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            ViewBag.hotel_id = new SelectList(db.Hotels, "id", "name");
            ViewBag.hotelFloor_id = new SelectList(db.HotelFloors, "id", "id");
            ViewBag.roomBand_id = new SelectList(db.RoomBands, "id", "name");
            ViewBag.roomPrice_id = new SelectList(db.RoomPrices, "id", "price");
            ViewBag.roomType_id = new SelectList(db.RoomTypes, "id", "name");
            return View();
        }

        // POST: Rooms/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,hotel_id,hotelFloor_id,roomType_id,roomBand_id,roomPrice_id,additionalNotes,deleted")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Rooms.Add(room);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.hotel_id = new SelectList(db.Hotels, "id", "name", room.hotel_id);
            ViewBag.hotelFloor_id = new SelectList(db.HotelFloors, "id", "id", room.hotelFloor_id);
            ViewBag.roomBand_id = new SelectList(db.RoomBands, "id", "name", room.roomBand_id);
            ViewBag.roomPrice_id = new SelectList(db.RoomPrices, "id", "id", room.roomPrice_id);
            ViewBag.roomType_id = new SelectList(db.RoomTypes, "id", "name", room.roomType_id);
            return View(room);
        }

        public ActionResult RemoveFacilityFromRoom(int id, int facilityId)
        {
            RoomFacility facility = db.RoomFacilities.Where(v => v.room_id == id && v.facility_id == facilityId).FirstOrDefault();

            if (facility != null)
            {
                facility.deleted = true;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = id });
            }
            return HttpNotFound();
        }

        public ActionResult AddFacility(int id)
        {
            Room room = db.Rooms.Find(id);

            List<Facility> currFacilities = room.RoomFacilities.Where(v => !v.deleted).Select(vv => vv.Facility).ToList();

            FacilityType type = currFacilities.Count() > 0 ? currFacilities.First().FacilityType : db.FacilityTypes.Where(v => !v.deleted && v.name == "Room").FirstOrDefault();

            List<Facility> roomFacilities = db.Facilities.Where(v => !v.deleted && v.facilityType_id == type.id).ToList();

            foreach (Facility f in currFacilities)
            {
                roomFacilities.Remove(f);
            }

            ViewBag.FacilityId = new SelectList(roomFacilities.ToList(), "id", "name");
            Session["RoomId"] = id;

            return View(new RoomFacilityVM { RoomId = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddFacility(RoomFacilityVM roomFacility)
        {
            int id = (int)Session["RoomId"];
            if (ModelState.IsValid)
            {
                db.RoomFacilities.Add(new RoomFacility { facility_id = roomFacility.FacilityId, room_id = id });
                db.SaveChanges();
                Session["RoomId"] = null;
                return RedirectToAction("Details", new { id });
            }

            Room room = db.Rooms.Find(id);
            List<Facility> currFacilities = room.RoomFacilities.Where(v => !v.deleted).Select(vv => vv.Facility).ToList();

            FacilityType type = currFacilities.Count() > 0 ? currFacilities.First().FacilityType : db.FacilityTypes.Where(v => !v.deleted && v.name == "Room").FirstOrDefault();

            List<Facility> roomFacilities = db.Facilities.Where(v => !v.deleted && v.facilityType_id == type.id).ToList();

            foreach (Facility f in currFacilities)
            {
                roomFacilities.Remove(f);
            }

            ViewBag.FacilityId = new SelectList(roomFacilities.ToList(), "id", "name", roomFacility.FacilityId);
            return View(room);
        }

        // GET: Rooms/Edit/5
        public ActionResult Edit(int? id)
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
            ViewBag.hotel_id = new SelectList(db.Hotels, "id", "name", room.hotel_id);
            ViewBag.hotelFloor_id = new SelectList(db.HotelFloors, "id", "id", room.hotelFloor_id);
            ViewBag.roomBand_id = new SelectList(db.RoomBands, "id", "name", room.roomBand_id);
            ViewBag.roomPrice_id = new SelectList(db.RoomPrices, "id", "id", room.roomPrice_id);
            ViewBag.roomType_id = new SelectList(db.RoomTypes, "id", "name", room.roomType_id);
            return View(room);
        }

        // POST: Rooms/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,hotel_id,hotelFloor_id,roomType_id,roomBand_id,roomPrice_id,additionalNotes,deleted")] Room room)
        {
            if (ModelState.IsValid)
            {
                db.Entry(room).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.hotel_id = new SelectList(db.Hotels, "id", "name", room.hotel_id);
            ViewBag.hotelFloor_id = new SelectList(db.HotelFloors, "id", "id", room.hotelFloor_id);
            ViewBag.roomBand_id = new SelectList(db.RoomBands, "id", "name", room.roomBand_id);
            ViewBag.roomPrice_id = new SelectList(db.RoomPrices, "id", "id", room.roomPrice_id);
            ViewBag.roomType_id = new SelectList(db.RoomTypes, "id", "name", room.roomType_id);
            return View(room);
        }

        // GET: Rooms/Delete/5
        public ActionResult Delete(int? id)
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
            return View(room);
        }

        // POST: Rooms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Room room = db.Rooms.Find(id);
            room.deleted = true;
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
