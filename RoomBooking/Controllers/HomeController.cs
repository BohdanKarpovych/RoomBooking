using System;
using System.Web.Mvc;
using RoomBooking.Models;
using System.Collections.Generic;
using RoomBooking.ServiceClasses;
using System.Linq;
using RoomBooking.Repositories;

namespace RoomBooking.Controllers
{
    public class HomeController : Controller
    {
        RoomBookingRepository db;

        public HomeController()
        {
            db = new RoomBookingRepository();
        }

        public ActionResult Index()
        {
            return View(db.GetRoomList());
        }

        [HttpGet]
        public ActionResult Book(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                List<Booking> bookings = db.GetBookingList();
                List<Room> rooms = db.GetRoomList();
                ViewBag.RoomNumber = rooms.Find(x => x.RoomId == id).RoomNumber;
                ViewBag.RoomId = id;
                var temp = bookings.FindAll(x => x.RoomId == id && (x.StartOfSession >= timeSpan || x.EndOfSession >= timeSpan));
                temp.Sort((a, b) => a.StartOfSession.CompareTo(b.StartOfSession));
                ViewBag.RoomSchedule = temp;
                return View();
            }
            else
            {
               return Content("<script>window.location = '/Account/Login';</script>");
            }
        }

        [HttpPost]
        public string Book(Booking booking, int Duration)
        {
            booking.EndOfSession = booking.StartOfSession + new TimeSpan(0, Duration, 0);
            List<Booking> bookings = db.GetBookingList();
            var a = bookings.FindAll(x => x.RoomId == booking.RoomId);
            Check check = new Check();
            if (check.CheckingBooking(booking, a))
            {
                db.Create(booking);
                db.Save();
                return "Your order has been succesfully done!";
            }
            return "Time is already occupated or passed";
        }

        [HttpGet]
        public ActionResult Find()
        {
            return View("Find");
        }

        [HttpPost]
        public ActionResult Find(string keyword)
        {
            List<Room> rooms = db.GetRoomList().FindAll(x => x.RoomNumber.Contains(keyword));
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