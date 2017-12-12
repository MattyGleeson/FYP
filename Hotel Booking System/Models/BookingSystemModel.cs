namespace Hotel_Booking_System.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class BookingSystemModel : DbContext
    {
        public BookingSystemModel()
            : base("name=BookingSystemModel")
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<Facility> Facilities { get; set; }
        public virtual DbSet<FacilityType> FacilityTypes { get; set; }
        public virtual DbSet<Guest> Guests { get; set; }
        public virtual DbSet<Hotel> Hotels { get; set; }
        public virtual DbSet<HotelFacility> HotelFacilities { get; set; }
        public virtual DbSet<HotelFloor> HotelFloors { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethods { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<RoomBand> RoomBands { get; set; }
        public virtual DbSet<RoomBooking> RoomBookings { get; set; }
        public virtual DbSet<RoomFacility> RoomFacilities { get; set; }
        public virtual DbSet<RoomPrice> RoomPrices { get; set; }
        public virtual DbSet<RoomType> RoomTypes { get; set; }
        public virtual DbSet<Title> Titles { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .Property(e => e.paymentTotal)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Booking>()
                .Property(e => e.comments)
                .IsUnicode(false);

            modelBuilder.Entity<Booking>()
                .HasMany(e => e.Guests)
                .WithRequired(e => e.Booking)
                .HasForeignKey(e => e.booking_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Booking>()
                .HasMany(e => e.Payments)
                .WithRequired(e => e.Booking)
                .HasForeignKey(e => e.booking_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Booking>()
                .HasMany(e => e.RoomBookings)
                .WithRequired(e => e.Booking)
                .HasForeignKey(e => e.booking_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.forename)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.surname)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.addressStreet)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.addressTown)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.addressCounty)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.addressPostalCode)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.homePhoneNo)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.workPhoneNo)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.mobilePhoneNo)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Bookings)
                .WithRequired(e => e.Customer)
                .HasForeignKey(e => e.customer_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Customer>()
                .HasMany(e => e.Payments)
                .WithRequired(e => e.Customer)
                .HasForeignKey(e => e.customer_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Facility>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Facility>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<Facility>()
                .HasMany(e => e.HotelFacilities)
                .WithRequired(e => e.Facility)
                .HasForeignKey(e => e.facility_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Facility>()
                .HasMany(e => e.RoomFacilities)
                .WithRequired(e => e.Facility)
                .HasForeignKey(e => e.facility_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<FacilityType>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<FacilityType>()
                .HasMany(e => e.Facilities)
                .WithRequired(e => e.FacilityType)
                .HasForeignKey(e => e.facilityType_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Guest>()
                .Property(e => e.forename)
                .IsUnicode(false);

            modelBuilder.Entity<Guest>()
                .Property(e => e.surname)
                .IsUnicode(false);

            modelBuilder.Entity<Guest>()
                .Property(e => e.addressStreet)
                .IsUnicode(false);

            modelBuilder.Entity<Guest>()
                .Property(e => e.addressTown)
                .IsUnicode(false);

            modelBuilder.Entity<Guest>()
                .Property(e => e.addressCounty)
                .IsUnicode(false);

            modelBuilder.Entity<Guest>()
                .Property(e => e.addressPostalCode)
                .IsUnicode(false);

            modelBuilder.Entity<Guest>()
                .Property(e => e.contactPhoneNo)
                .IsUnicode(false);

            modelBuilder.Entity<Hotel>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Hotel>()
                .Property(e => e.phoneNo)
                .IsUnicode(false);

            modelBuilder.Entity<Hotel>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<Hotel>()
                .HasMany(e => e.HotelFacilities)
                .WithRequired(e => e.Hotel)
                .HasForeignKey(e => e.hotel_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Hotel>()
                .HasMany(e => e.HotelFloors)
                .WithRequired(e => e.Hotel)
                .HasForeignKey(e => e.hotel_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Hotel>()
                .HasMany(e => e.Rooms)
                .WithRequired(e => e.Hotel)
                .HasForeignKey(e => e.hotel_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<HotelFloor>()
                .HasMany(e => e.Rooms)
                .WithRequired(e => e.HotelFloor)
                .HasForeignKey(e => e.hotelFloor_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Payment>()
                .Property(e => e.amount)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Payment>()
                .Property(e => e.comments)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMethod>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<PaymentMethod>()
                .HasMany(e => e.Payments)
                .WithRequired(e => e.PaymentMethod)
                .HasForeignKey(e => e.paymentMethod_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Room>()
                .Property(e => e.additionalNotes)
                .IsUnicode(false);

            modelBuilder.Entity<Room>()
                .HasMany(e => e.RoomBookings)
                .WithRequired(e => e.Room)
                .HasForeignKey(e => e.room_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Room>()
                .HasMany(e => e.RoomFacilities)
                .WithRequired(e => e.Room)
                .HasForeignKey(e => e.room_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RoomBand>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<RoomBand>()
                .HasMany(e => e.Rooms)
                .WithRequired(e => e.RoomBand)
                .HasForeignKey(e => e.roomBand_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RoomPrice>()
                .Property(e => e.price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<RoomPrice>()
                .HasMany(e => e.Rooms)
                .WithRequired(e => e.RoomPrice)
                .HasForeignKey(e => e.roomPrice_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<RoomType>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<RoomType>()
                .HasMany(e => e.Rooms)
                .WithRequired(e => e.RoomType)
                .HasForeignKey(e => e.roomType_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Title>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<Title>()
                .HasMany(e => e.Customers)
                .WithRequired(e => e.Title)
                .HasForeignKey(e => e.title_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Title>()
                .HasMany(e => e.Guests)
                .WithRequired(e => e.Title)
                .HasForeignKey(e => e.title_id)
                .WillCascadeOnDelete(false);
        }
    }
}
