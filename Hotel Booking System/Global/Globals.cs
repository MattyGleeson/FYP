using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Hotel_Booking_System.Global
{
    public static class Globals
    {
        public static readonly String StartDateSessionVar = "StartDate";
        public static readonly String EndDateSessionVar = "EndDate";
        public static readonly String CartSessionVar = "Cart";
        public static readonly String BookingSessionVar = "Booking";
        public static readonly IEnumerable<String> Titles = new List<String>() { "Mr", "Mrs", "Ms", "Dr" };
        public static readonly IEnumerable<String> BookingPeriods = new List<String>() { "Created On", "Start Date", "End Date" };
        public static readonly IEnumerable<String> PaymentStatuses = new List<String>() { "Paid", "Un-Paid" };

        private static readonly String ConStringName = "BookingSystemModel";

        public static String ConnectionString()
        {
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings[ConStringName];

            if (mySetting == null || string.IsNullOrEmpty(mySetting.ConnectionString))
                throw new Exception("Fatal error: missing connecting string in web.config file");

            return mySetting.ConnectionString;
        }
    }
}