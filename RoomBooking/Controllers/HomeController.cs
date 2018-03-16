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

        //TODO: Finish normal datepicking
        [HttpGet]
        public ActionResult Book(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                var timeNow = DateTime.Now;
                List<Booking> bookings = db.GetBookingList();
                List<Room> rooms = db.GetRoomList();
                ViewBag.RoomNumber = rooms.Find(x => x.RoomId == id).RoomNumber;
                ViewBag.RoomId = id;
                using (RoomBookingAuthRepository dbauth = new RoomBookingAuthRepository())
                {
                    ViewBag.UserId = dbauth.GetUserId(User.Identity.Name);

                }
                var temp = bookings.FindAll(x => x.RoomId == id && (x.StartOfSession >= timeNow || x.EndOfSession >= timeNow));
                temp.Sort((a, b) => a.StartOfSession.CompareTo(b.StartOfSession));
                ViewBag.RoomSchedule = temp;
                return View();
                //TimeSpan timeSpan = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, DateTime.Now.Second);
                //List<Booking> bookings = db.GetBookingList();
                //List<Room> rooms = db.GetRoomList();
                //ViewBag.RoomNumber = rooms.Find(x => x.RoomId == id).RoomNumber;
                //ViewBag.RoomId = id;
                //using (RoomBookingAuthRepository dbauth = new RoomBookingAuthRepository())
                //{
                //    ViewBag.UserId = dbauth.GetUserId(User.Identity.Name);
                //}
                //var temp = bookings.FindAll(x => x.RoomId == id && (x.StartOfSession >= timeSpan || x.EndOfSession >= timeSpan));
                //temp.Sort((a, b) => a.StartOfSession.CompareTo(b.StartOfSession));
                //ViewBag.RoomSchedule = temp;
                //return View();
            }
            else
            {
                return Content("<script>window.location = '/Account/Login';</script>");
            }
        }

        [HttpPost]
        public ActionResult Book(Booking booking, int Duration, string Date)
        {
            //var _date = Date.Split('/');
            //booking.Date = new DateTime(int.Parse(_date[2]), int.Parse(_date[0]), int.Parse(_date[1]), booking.StartOfSession.Hours, booking.StartOfSession.Minutes, 0);
            //booking.EndOfSession = booking.StartOfSession + new TimeSpan(0, Duration, 0);
            //List<Booking> bookings = db.GetBookingList();
            //var a = bookings.FindAll(x => x.RoomId == booking.RoomId);
            //Check check = new Check();
            //if (check.CheckingBooking(booking.Date, a))
            //{
            //    db.Create(booking);
            //    db.Save();
            //    ViewBag.Message = "Your order has been succesfully done!";
            //    return View("OrderResult");
            //}
            ViewBag.Message = "Time is already occupated or passed";
            return View("OrderResult");
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