namespace Hotel_Booking_System.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Booking")]
    public partial class Booking
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Booking()
        {
            Payments = new HashSet<Payment>();
            RoomBookings = new HashSet<RoomBooking>();
        }

        public int id { get; set; }

        public int customer_id { get; set; }

        public int? guest_id { get; set; }

        [Column(TypeName = "date")]
        public DateTime bookingMadeDate { get; set; }

        public TimeSpan bookingMadeTime { get; set; }

        [Column(TypeName = "date")]
        public DateTime startDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime endDate { get; set; }

        public decimal paymentTotal { get; set; }

        [Column(TypeName = "date")]
        public DateTime paymentDueDate { get; set; }

        [Column(TypeName = "date")]
        public DateTime? paymentMadeDate { get; set; }

        [StringLength(150)]
        public string comments { get; set; }

        public bool cancelled { get; set; }

        public bool deleted { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Guest Guest { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RoomBooking> RoomBookings { get; set; }
    }
}
