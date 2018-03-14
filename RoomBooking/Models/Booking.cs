using System;

namespace RoomBooking.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
        //public string FirstName { get; set; }
        //public string LastName { get; set; }
        public TimeSpan StartOfSession { get; set; }
        public TimeSpan EndOfSession { get; set; }

        public Room Room { get; set; }
        public User User { get; set; }
    }
}