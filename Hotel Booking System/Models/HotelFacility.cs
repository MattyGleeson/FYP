namespace Hotel_Booking_System.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelFacility")]
    public partial class HotelFacility
    {
        public int id { get; set; }

        public int facility_id { get; set; }

        public int hotel_id { get; set; }

        public bool deleted { get; set; }

        public virtual Facility Facility { get; set; }

        public virtual Hotel Hotel { get; set; }
    }
}
