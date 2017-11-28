namespace Hotel_Booking_System.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HotelFloor")]
    public partial class HotelFloor
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public HotelFloor()
        {
            Rooms = new HashSet<Room>();
        }

        public int id { get; set; }

        public int no { get; set; }

        public int hotel_id { get; set; }

        public bool deleted { get; set; }

        public virtual Hotel Hotel { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
