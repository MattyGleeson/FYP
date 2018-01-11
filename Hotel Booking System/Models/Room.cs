namespace Hotel_Booking_System.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Room")]
    public partial class Room
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Room()
        {
            RoomBookings = new HashSet<RoomBooking>();
            RoomFacilities = new HashSet<RoomFacility>();
        }

        public int id { get; set; }

        public int hotel_id { get; set; }

        public int hotelFloor_id { get; set; }

        public int roomType_id { get; set; }

        public int roomBand_id { get; set; }

        public int roomPrice_id { get; set; }

        [StringLength(150)]
        public string additionalNotes { get; set; }

        public bool active { get; set; }

        public bool deleted { get; set; }

        public virtual Hotel Hotel { get; set; }

        public virtual HotelFloor HotelFloor { get; set; }

        public virtual RoomBand RoomBand { get; set; }

        public virtual RoomPrice RoomPrice { get; set; }

        public virtual RoomType RoomType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoomBooking> RoomBookings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoomFacility> RoomFacilities { get; set; }
    }
}
