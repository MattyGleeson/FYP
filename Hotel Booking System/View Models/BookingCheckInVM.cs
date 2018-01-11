using Hotel_Booking_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel_Booking_System.View_Models
{
    public class BookingCheckInVM
    {
        public int Id { get; set; }

        public int? CustomerId { get; set; }

        public string Title { get; set; }

        public string Forename { get; set; }

        public string Surname { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(
            DataFormatString = "{0:dd-MM-yyyy}",
            ApplyFormatInEditMode = true
            )]
        public DateTime? DoB { get; set; }

        public string AddressStreet { get; set; }

        public string AddressTown { get; set; }

        public string AddressCounty { get; set; }

        public string AddressPostalCode { get; set; }

        public string ContactPhoneNo { get; set; }

        public bool IsCustomerFromBooking { get; set; }

        public Booking Booking { get; set; }

        public int BookingId { get; set; }

        public string ContactPhoneNoSelected { get; set; }

        public string BookingCustomerDoB { get; set; }
    }
}