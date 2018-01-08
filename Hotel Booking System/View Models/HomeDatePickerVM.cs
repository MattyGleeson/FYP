using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel_Booking_System.View_Models
{
    public class HomeDatePickerVM
    {
        [DataType(DataType.Date)]
        [DisplayFormat(
            DataFormatString = "{0:dd-MM-yyyy}",
            ApplyFormatInEditMode = true
            )]
        public DateTime StartDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(
            DataFormatString = "{0:dd-MM-yyyy}",
            ApplyFormatInEditMode = true
            )]
        public DateTime EndDate { get; set; }
    }
}