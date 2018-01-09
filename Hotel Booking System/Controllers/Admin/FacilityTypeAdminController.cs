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
    public class FacilityTypeAdminController : MessageControllerBase
    {
        private BookingSystemModel db = new BookingSystemModel();

        // GET: FacilityTypes
        public ActionResult Index()
        {
            return View(db.FacilityTypes.Where(v => !v.deleted).ToList());
        }

        // GET: FacilityTypes/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: FacilityTypes/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,name,deleted")] FacilityType facilityType)
        {
            if (ModelState.IsValid)
            {
                db.FacilityTypes.Add(facilityType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(facilityType);
        }

        // GET: FacilityTypes/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacilityType facilityType = db.FacilityTypes.Find(id);
            if (facilityType == null)
            {
                return HttpNotFound();
            }
            return View(facilityType);
        }

        // POST: FacilityTypes/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,name,deleted")] FacilityType facilityType)
        {
            if (ModelState.IsValid)
            {
                db.Entry(facilityType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(facilityType);
        }

        // GET: FacilityTypes/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            FacilityType facilityType = db.FacilityTypes.Find(id);
            if (facilityType == null)
            {
                return HttpNotFound();
            }
            return View(facilityType);
        }

        // POST: FacilityTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FacilityType facilityType = db.FacilityTypes.Find(id);
            facilityType.deleted = true;
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
