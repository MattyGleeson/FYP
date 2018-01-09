namespace Hotel_Booking_System.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Customer")]
    public partial class Customer
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Customer()
        {
            Bookings = new HashSet<Booking>();
            Guests = new HashSet<Guest>();
            Payments = new HashSet<Payment>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(4)]
        public string title { get; set; }

        [Required]
        [StringLength(20)]
        public string forename { get; set; }

        [Required]
        [StringLength(25)]
        public string surname { get; set; }

        [Column(TypeName = "date")]
        public DateTime dob { get; set; }

        [Required]
        [StringLength(100)]
        public string addressStreet { get; set; }

        [Required]
        [StringLength(50)]
        public string addressTown { get; set; }

        [Required]
        [StringLength(50)]
        public string addressCounty { get; set; }

        [Required]
        [StringLength(8)]
        public string addressPostalCode { get; set; }

        [Required]
        [StringLength(15)]
        public string homePhoneNo { get; set; }

        [StringLength(15)]
        public string workPhoneNo { get; set; }

        [Required]
        [StringLength(15)]
        public string mobilePhoneNo { get; set; }

        [Required]
        [StringLength(50)]
        public string email { get; set; }

        public bool deleted { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Booking> Bookings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Guest> Guests { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payment> Payments { get; set; }
    }
}
