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
            return View(db.Rooms);
        }

        [HttpGet]
        public ActionResult Book(int id)
        {
            TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
            List<Booking> bookings = db.Bookings.ToList();
            List<Room> rooms = db.Rooms.ToList();
            ViewBag.RoomNumber = rooms.Find(x => x.RoomId == id).RoomNumber;
            ViewBag.RoomId = id;
            var temp = bookings.FindAll(x => x.RoomId == id && (x.StartOfSession >= timeSpan || x.EndOfSession >= timeSpan));
            temp.Sort((a, b) => a.StartOfSession.CompareTo(b.StartOfSession));
            ViewBag.RoomSchedule = temp;
            return View();
        }

        [HttpPost]
        public string Book(Booking booking, int Duration)
        {
            booking.EndOfSession = booking.StartOfSession + new TimeSpan(0, Duration, 0);
            if (CheckingBooking(booking))
            {
                db.Bookings.Add(booking);
                db.SaveChanges();
                return "Your order has been succesfully done!";
            }
            return "Time is already occupated or passed";
        }

        private bool CheckingBooking(Booking booking)
        {
            TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);

            if (booking.StartOfSession > timeSpan)
            {
                List<Booking> bookings = db.Bookings.ToList();
                var a = bookings.FindAll(x => x.RoomId == booking.RoomId);
                foreach (var item in a)
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

        [HttpGet]
        public ActionResult Find()
        {
            return View("Find");
        }

        [HttpPost]
        public ActionResult Find(string keyword)
        {
            List<Room> rooms = db.Rooms.ToList().FindAll(x => x.RoomNumber.Contains(keyword));
            ViewBag.Rooms = rooms;
            return View("SearchResults");
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