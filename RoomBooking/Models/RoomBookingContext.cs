using System;
using System.Data.Entity;

namespace RoomBooking.Models
{
    public class RoomBookingContext : DbContext
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
}