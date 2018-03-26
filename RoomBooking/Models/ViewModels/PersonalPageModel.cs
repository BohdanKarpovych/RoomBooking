using System;
using System.Collections.Generic;

namespace RoomBooking.Models.ViewModels
{
    public class PersonalPageModel
    {
        public List<Item> BookingList { get; set; }

        public PersonalPageModel()
        {
            BookingList = new List<Item>();
        }
    }

    public class Item
    {
        public string RoomNumber { get; set; }
        public DateTime StartOfSession { get; set; }
        public DateTime EndOfSession { get; set; }
    }
}