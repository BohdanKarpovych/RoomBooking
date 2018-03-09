using System;

namespace RoomBooking.Models
{
    public class Booking
    {
        public int BookingId { get; set; }
        public int RoomId { get; set; }
        public int PersonId { get; set; }
        public DateTime StartOfSession { get; set; }
        public DateTime EndOfSession { get; set; }
    }
}