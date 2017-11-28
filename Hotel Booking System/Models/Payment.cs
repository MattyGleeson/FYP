namespace Hotel_Booking_System.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Payment")]
    public partial class Payment
    {
        public int id { get; set; }

        public int paymentMethod_id { get; set; }

        public int customer_id { get; set; }

        public int booking_id { get; set; }

        public decimal amount { get; set; }

        [StringLength(150)]
        public string comments { get; set; }

        public bool deleted { get; set; }

        public virtual Booking Booking { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual PaymentMethod PaymentMethod { get; set; }
    }
}
