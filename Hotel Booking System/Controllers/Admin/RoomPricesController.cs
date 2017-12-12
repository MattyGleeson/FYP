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
    public class RoomPricesController : Controller
    {
        private BookingSystemModel db = new BookingSystemModel();

        // GET: RoomPrices
        public ActionResult Index()
        {
            return View(db.RoomPrices.ToList());
        }

        // GET: RoomPrices/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoomPrices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,price,deleted")] RoomPrice roomPrice)
        {
            if (ModelState.IsValid)
            {
                db.RoomPrices.Add(roomPrice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roomPrice);
        }

        // GET: RoomPrices/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomPrice roomPrice = db.RoomPrices.Find(id);
            if (roomPrice == null)
            {
                return HttpNotFound();
            }
            return View(roomPrice);
        }

        // POST: RoomPrices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,price,deleted")] RoomPrice roomPrice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomPrice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roomPrice);
        }

        // GET: RoomPrices/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomPrice roomPrice = db.RoomPrices.Find(id);
            if (roomPrice == null)
            {
                return HttpNotFound();
            }
            return View(roomPrice);
        }

        // POST: RoomPrices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomPrice roomPrice = db.RoomPrices.Find(id);
            roomPrice.deleted = true;
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
