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

namespace Hotel_Booking_System.Controllers.Admin
{
    public class RoomBandAdminController : MessageControllerBase
    {
        private BookingSystemModel db = new BookingSystemModel();

        // GET: RoomBands
        public ActionResult Index()
        {
            return View(db.RoomBands.Where(v => !v.deleted).ToList());
        }

        // GET: RoomBands/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: RoomBands/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,deleted")] RoomBand roomBand)
        {
            if (ModelState.IsValid)
            {
                db.RoomBands.Add(roomBand);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(roomBand);
        }

        // GET: RoomBands/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomBand roomBand = db.RoomBands.Find(id);
            if (roomBand == null)
            {
                return HttpNotFound();
            }
            return View(roomBand);
        }

        // POST: RoomBands/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,deleted")] RoomBand roomBand)
        {
            if (ModelState.IsValid)
            {
                db.Entry(roomBand).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(roomBand);
        }

        // GET: RoomBands/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            RoomBand roomBand = db.RoomBands.Find(id);
            if (roomBand == null)
            {
                return HttpNotFound();
            }
            return View(roomBand);
        }

        // POST: RoomBands/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            RoomBand roomBand = db.RoomBands.Find(id);
            roomBand.deleted = true;
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
