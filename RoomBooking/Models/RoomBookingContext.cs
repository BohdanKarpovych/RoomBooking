using System;
using System.Data.Entity;

namespace RoomBooking.Models
{
    public class RoomBookingContext : DbContext
    {
        public RoomBookingContext() : base("RoomBookingContext")
        {
        }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<User> Users { get; set; }
    }
}