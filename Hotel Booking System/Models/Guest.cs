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
        public int id { get; set; }

        public int title_id { get; set; }

        public int booking_id { get; set; }

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
        public string contactPhoneNo { get; set; }

        public bool deleted { get; set; }

        public virtual Booking Booking { get; set; }

        public virtual Title Title { get; set; }
    }
}
