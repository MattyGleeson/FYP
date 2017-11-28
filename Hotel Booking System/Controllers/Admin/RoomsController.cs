using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Hotel_Booking_System.Models;

namespace Hotel_Booking_System.Controllers.Admin
{
    public class RoomsController : Controller
    {
        private BookingSystemModel db = new BookingSystemModel();

        // GET: Rooms
        public ActionResult Index()
        {
            var rooms = db.Rooms.Include(r => r.Hotel).Include(r => r.HotelFloor).Include(r => r.RoomBand).Include(r => r.RoomPrice).Include(r => r.RoomType);
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
            return View(room);
        }

        // GET: Rooms/Create
        public ActionResult Create()
        {
            ViewBag.hotel_id = new SelectList(db.Hotels, "id", "name");
            ViewBag.hotelFloor_id = new SelectList(db.HotelFloors, "id", "id");
            ViewBag.roomBand_id = new SelectList(db.RoomBands, "id", "name");
            ViewBag.roomPrice_id = new SelectList(db.RoomPrices, "id", "id");
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
            db.Rooms.Remove(room);
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
