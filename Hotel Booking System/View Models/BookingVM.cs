using Hotel_Booking_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel_Booking_System.View_Models
{
    public class BookingVM
    {
        public int ModelId { get; set; }

        public String Name { get; set; }

        public String Created { get; set; }

        public String From { get; set; }

        public String To { get; set; }

        public int NoRooms { get; set; }

        public String Total { get; set; }

        public bool PaymentMade { get; set; }

        public bool IsGuest { get; set; }
    }
}