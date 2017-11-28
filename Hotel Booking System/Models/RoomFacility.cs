namespace Hotel_Booking_System.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoomFacility")]
    public partial class RoomFacility
    {
        public int id { get; set; }

        public int facility_id { get; set; }

        public int room_id { get; set; }

        public bool deleted { get; set; }

        public virtual Facility Facility { get; set; }

        public virtual Room Room { get; set; }
    }
}
