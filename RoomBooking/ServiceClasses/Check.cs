using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoomBooking.Models;

namespace RoomBooking.ServiceClasses
{
    public class Check
    {
        TimeSpan timeSpan;

        public bool CheckingBooking(Booking booking, List<Booking> list)
        {
            timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            if (booking.StartOfSession > timeSpan)
            {
                foreach (var item in list)
                {
                    if (booking.StartOfSession <= item.EndOfSession)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }
    }
}