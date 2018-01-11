using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Hotel_Booking_System.View_Models
{
    public class CreateCustomerVM
    {
        [Required]
        [StringLength(4)]
        public String Title { get; set; }

        [Required]
        [StringLength(20)]
        public String Forename { get; set; }

        [Required]
        [StringLength(25)]
        public String Surname { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(
            DataFormatString = "{0:dd-MM-yyyy}",
            ApplyFormatInEditMode = true
            )]
        public DateTime DoB { get; set; }

        [Required]
        public String HouseNo { get; set; }

        [Required]
        [StringLength(100)]
        public String Street { get; set; }

        [Required]
        [StringLength(50)]
        public String Town { get; set; }

        [Required]
        [StringLength(50)]
        public String County { get; set; }

        [Required]
        [StringLength(8)]
        public String PostCode { get; set; }

        [Required]
        [StringLength(15)]
        public String HomePhoneNo { get; set; }

        [StringLength(15)]
        public String WorkPhoneNo { get; set; }

        [Required]
        [StringLength(15)]
        public String MobPhoneNo { get; set; }

        [Required]
        [StringLength(50)]
        public String Email { get; set; }
    }
}