using System.Collections.Generic;

namespace RoomBooking.Models.ViewModels
{
    public class BookModel
    {
        public List<Booking> RoomSchedule { get; set; }
        public string RoomNumber { get; set; }
        public int RoomId { get; set; }
        public int UserId { get; set; }
    }
}