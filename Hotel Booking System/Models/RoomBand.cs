namespace Hotel_Booking_System.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("RoomBand")]
    public partial class RoomBand
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public RoomBand()
        {
            Rooms = new HashSet<Room>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(20)]
        public string name { get; set; }

        public bool deleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Room> Rooms { get; set; }
    }
}
