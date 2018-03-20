using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoomBooking.Models;

namespace RoomBooking.ServiceClasses
{
    public class Check
    {
        private TimeSpan StartWork;
        private TimeSpan EndWork;

        public Check()
        {
            StartWork = new TimeSpan(9, 0, 0);
            EndWork = new TimeSpan(19, 0, 0);
        }

        public bool CheckingBooking(Booking booking, List<Booking> list)
        {
            var timeNow = DateTime.Now;
            if (IsInWorkingRange(booking.StartOfSession) &&
                IsInWorkingRange(booking.EndOfSession) &&
                booking.StartOfSession > timeNow)
            {
                foreach (var item in list)
                {
                    if (booking.StartOfSession <= item.EndOfSession && booking.StartOfSession >= item.StartOfSession &&
                        booking.EndOfSession <= item.EndOfSession && booking.EndOfSession >= item.StartOfSession)
                    {
                        return false;
                    }
                }
                return true;
            }
            return false;
        }

        public bool IsInWorkingRange(DateTime dateTime)
        {
            if (dateTime.Hour >= StartWork.Hours &&
                dateTime.Hour <= EndWork.Hours)
            {
                return true;
            }
            return false;
        }
    }
}