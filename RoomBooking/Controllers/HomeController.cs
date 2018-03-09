using System;
using System.Web.Mvc;
using RoomBooking.Models;
using System.Collections.Generic;

namespace RoomBooking.Controllers
{
    public class HomeController : Controller
    {
        RoomBookingContext db = new RoomBookingContext();

        public ActionResult Index()
        {
            IEnumerable<Room> rooms = db.Rooms;
            ViewBag.Rooms = rooms;
            return View();
        }

        [HttpGet]
        public ActionResult Book(int id)
        {
            ViewBag.RoomId = id;

            return View();
        }

        //[HttpPost]
        //public string Book(Booking booking)
        //{
            
        //}

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