using Hotel_Booking_System.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace Hotel_Booking_System.Global
{
    public static class StoredProcedures
    {
        private static BookingSystemModel db = new BookingSystemModel();

        public static IEnumerable<Room> GetBookingRooms(int id)
        {
            List<Room> res = new List<Room>();

            try
            {
                using (SqlConnection con = new SqlConnection(Globals.ConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("getBookingRooms", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@bookingId", SqlDbType.Int).Value = id;
                        con.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Room room = new Room
                                {
                                    id = Convert.ToInt32(reader[0]),
                                    hotel_id = Convert.ToInt32(reader[1]),
                                    hotelFloor_id = Convert.ToInt32(reader[2]),
                                    roomType_id = Convert.ToInt32(reader[3]),
                                    roomBand_id = Convert.ToInt32(reader[4]),
                                    roomPrice_id = Convert.ToInt32(reader[5]),
                                    additionalNotes = Convert.ToString(reader[6])
                                };

                                room.Hotel = db.Hotels.Find(room.hotel_id);
                                room.HotelFloor = db.HotelFloors.Find(room.hotelFloor_id);
                                room.RoomType = db.RoomTypes.Find(room.roomType_id);
                                room.RoomBand = db.RoomBands.Find(room.roomBand_id);
                                room.RoomPrice = db.RoomPrices.Find(room.roomPrice_id);

                                res.Add(room);
                            }
                            return res.AsQueryable().Include(b => b.Hotel).Include(b => b.HotelFloor).Include(b => b.RoomType).Include(b => b.RoomBand).Include(b => b.RoomPrice);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return res;
            }
        }

        public static bool DeleteBooking(int bookingId, int custId)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Globals.ConnectionString()))
                {
                    using (SqlCommand cmd = new SqlCommand("deleteBooking", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@bookingId", SqlDbType.Int).Value = bookingId;
                        cmd.Parameters.Add("@custId", SqlDbType.Int).Value = custId;
                        con.Open();
                        cmd.ExecuteNonQuery();
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}