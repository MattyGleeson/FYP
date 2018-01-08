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

        private static String ConStringName = "BookingSystemModel";

        public static String ConnectionString()
        {
            ConnectionStringSettings mySetting = ConfigurationManager.ConnectionStrings[ConStringName];
            if (mySetting == null || string.IsNullOrEmpty(mySetting.ConnectionString))
                throw new Exception("Fatal error: missing connecting string in web.config file");
            return mySetting.ConnectionString;
        }
    }
}