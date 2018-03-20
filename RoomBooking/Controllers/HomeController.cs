using System;
using System.Web.Mvc;
using RoomBooking.Models;
using System.Collections.Generic;
using RoomBooking.ServiceClasses;
using System.Linq;
using RoomBooking.Repositories;
using PagedList;

namespace RoomBooking.Controllers
{
    public class HomeController : Controller
    {
        RoomBookingRepository db;

        public HomeController()
        {
            db = new RoomBookingRepository();
        }

        public ActionResult Index(int? page)
        {
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(db.GetRoomList().ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult Book(int id)
        {
            if (User.Identity.IsAuthenticated)
            {
                List<Booking> bookings = db.GetActualBookingList();
                List<Room> rooms = db.GetRoomList();
                ViewBag.RoomNumber = rooms.Find(x => x.RoomId == id).RoomNumber;
                ViewBag.RoomId = id;
                using (RoomBookingAuthRepository dbauth = new RoomBookingAuthRepository())
                {
                    ViewBag.UserId = dbauth.GetUserId(User.Identity.Name);
                }
                var bookingsOfCurrentRoom = bookings.FindAll(x => x.RoomId == id);
                bookingsOfCurrentRoom.Sort((a, b) => a.StartOfSession.CompareTo(b.StartOfSession));
                ViewBag.RoomSchedule = bookingsOfCurrentRoom;
                return View();
            }
            else
            {
                return Content("<script>window.location = '/Account/Login';</script>");
            }
        }

        [HttpPost]
        public ActionResult Book(Booking booking, int Duration, string Date)
        {
            //Parse date
            var _date = Date.Split('/');
            booking.StartOfSession = new DateTime(int.Parse(_date[2]), int.Parse(_date[0]), int.Parse(_date[1]), booking.StartOfSession.Hour, booking.StartOfSession.Minute, booking.StartOfSession.Second);
            
            booking.EndOfSession = booking.StartOfSession.AddMinutes(Duration);
            List<Booking> bookings = db.GetBookingList();
            var a = bookings.FindAll(x => x.RoomId == booking.RoomId);
            Check check = new Check();
            if (check.CheckingBooking(booking, a))
            {
                db.Create(booking);
                db.Save();
                ViewBag.Status = true;
                ViewBag.Message = "Your order has been succesfully done!";
                return View("OrderResult");
            }
            ViewBag.Status = false;
            ViewBag.Message = "Time is unavailable";
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