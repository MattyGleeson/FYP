namespace Hotel_Booking_System.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoomBooking")]
    public partial class RoomBooking
    {
        public int id { get; set; }

        public int booking_id { get; set; }

        public int room_id { get; set; }

        public bool deleted { get; set; }

        public virtual Booking Booking { get; set; }

        public virtual Room Room { get; set; }
    }
}
