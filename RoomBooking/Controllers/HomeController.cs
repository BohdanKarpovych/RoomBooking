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
            IEnumerable<Booking> bookings = db.Bookings;
            ViewBag.Rooms = rooms;
            ViewBag.Bookings = bookings;
            return View();
        }

        [HttpGet]
        public ActionResult Book(int id)
        {
            ViewBag.RoomId = id;
            return View();
        }

        [HttpPost]
        public string Book(Booking booking)
        {
            purchase.Date = DateTime.Now;
            // добавляем информацию о покупке в базу данных
            db.Purchases.Add(purchase);
            // сохраняем в бд все изменения
            db.SaveChanges();
            return "Спасибо," + purchase.Person + ", за покупку!";
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