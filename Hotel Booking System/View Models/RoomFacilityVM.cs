using Hotel_Booking_System.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel_Booking_System.View_Models
{
    public class RoomFacilityVM
    {
        public int RoomId { get; set; }

        [Required]
        public int FacilityId { get; set; }
    }
}