using System;
using System.Web.Mvc;
using RoomBooking.Models;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RoomBooking.Controllers
{
    public class HomeController : Controller
    {
        RoomBookingContext db = new RoomBookingContext();

        public ActionResult Index()
        {
            ViewBag.Rooms = db.Rooms;
            return View();
        }

        [HttpGet]
        public ActionResult Book(int id)
        {
            TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            List<Booking> bookings = db.Bookings.ToList();
            List<Room> rooms = db.Rooms.ToList();
            ViewBag.RoomNumber = rooms.Find(x => x.RoomId == id).RoomNumber;
            ViewBag.RoomSchedule = bookings.FindAll(x => x.RoomId == id && (x.StartOfSession >= timeSpan || x.EndOfSession >= timeSpan));
            return View();
        }

        [HttpPost]
        public string Book(Booking booking)
        {
            TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            if (booking.StartOfSession < booking.EndOfSession)
            {
                if (booking.StartOfSession > timeSpan)
                {
                    List<Booking> bookings = db.Bookings.ToList();
                    var a = bookings.FindAll(x => x.RoomId == booking.RoomId && (x.StartOfSession >= timeSpan || x.EndOfSession >= timeSpan));
                    foreach (var item in a)
                    {
                        if (booking.StartOfSession <= item.EndOfSession)
                        {
                            return "Time is occupated";
                        }
                    }
                    db.Bookings.Add(booking);
                    db.SaveChanges();
                    return "Your order has been succesfully done!";
                }
                return "Input actual time";
            }
            return "Ending time has to be less than starting";
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}