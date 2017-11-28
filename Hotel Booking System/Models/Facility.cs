namespace Hotel_Booking_System.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Facility")]
    public partial class Facility
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Facility()
        {
            HotelFacilities = new HashSet<HotelFacility>();
            RoomFacilities = new HashSet<RoomFacility>();
        }

        public int id { get; set; }

        public int facilityType_id { get; set; }

        [Required]
        [StringLength(20)]
        public string name { get; set; }

        [StringLength(50)]
        public string description { get; set; }

        public bool deleted { get; set; }

        public virtual FacilityType FacilityType { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HotelFacility> HotelFacilities { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoomFacility> RoomFacilities { get; set; }
    }
}
