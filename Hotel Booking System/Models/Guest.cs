namespace Hotel_Booking_System.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Guest")]
    public partial class Guest
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Guest()
        {
            Bookings = new HashSet<Booking>();
        }

        public int id { get; set; }

        public int? customer_id { get; set; }

        [StringLength(4)]
        public string title { get; set; }

        [StringLength(20)]
        public string forename { get; set; }

        [StringLength(25)]
        public string surname { get; set; }

        [Column(TypeName = "date")]
        public DateTime? dob { get; set; }

        [StringLength(100)]
        public string addressStreet { get; set; }

        [StringLength(50)]
        public string addressTown { get; set; }

        [StringLength(50)]
        public string addressCounty { get; set; }

        [StringLength(8)]
        public string addressPostalCode { get; set; }

        [StringLength(15)]
        public string contactPhoneNo { get; set; }

        public bool deleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
