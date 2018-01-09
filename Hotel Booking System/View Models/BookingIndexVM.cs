using Hotel_Booking_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel_Booking_System.View_Models
{
    public class BookingIndexVM
    {
        public IEnumerable<BookingVM> Bookings { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(
            DataFormatString = "{0:dd-MM-yyyy}",
            ApplyFormatInEditMode = true
            )]
        public DateTime ChosenStartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(
            DataFormatString = "{0:dd-MM-yyyy}",
            ApplyFormatInEditMode = true
            )]
        public DateTime ChosenEndDate { get; set; }

        public String DateFilter { get; set; }

        public String Paid { get; set; }
    }
}