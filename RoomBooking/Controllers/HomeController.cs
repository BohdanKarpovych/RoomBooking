using System;
using System.Web.Mvc;
using RoomBooking.Models;
using System.Collections.Generic;
using RoomBooking.ServiceClasses;
using RoomBooking.Models.ViewModels;
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
            int pageSize = 5;
            int pageNumber = (page ?? 1);
            return View(db.GetRoomList().ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        [Authorize]
        public ActionResult Book(int? id)
        {
            if (id != null)
            {
                BookModel bookModel = new BookModel();
                bookModel.RoomNumber = db.GetRoomList().Find(x => x.RoomId == id).RoomNumber;
                bookModel.RoomId = (int)id;
                using (RoomBookingAuthRepository dbauth = new RoomBookingAuthRepository())
                {
                    bookModel.UserId = dbauth.GetUserId(User.Identity.Name);
                }
                var bookingsOfCurrentRoom = db.GetActualBookingList().FindAll(x => x.RoomId == id);
                bookingsOfCurrentRoom.Sort((a, b) => a.StartOfSession.CompareTo(b.StartOfSession));
                bookModel.RoomSchedule = bookingsOfCurrentRoom;
                return View(bookModel);
            }
            return Redirect("/Home/Index");
        }

        [HttpPost]
        public ActionResult Book(Booking booking, int Duration, string Date)
        {
            //Parse date
            var _date = Date.Split('/');
            try
            {
                booking.StartOfSession = new DateTime(int.Parse(_date[2]), int.Parse(_date[0]), int.Parse(_date[1]), booking.StartOfSession.Hour, booking.StartOfSession.Minute, booking.StartOfSession.Second);
            }
            catch (FormatException)
            {
                return View("OrderResult", new OrderResultModel() { Status = false, Message = "Date is invalid" });
            }
            booking.EndOfSession = booking.StartOfSession.AddMinutes(Duration);
            List<Booking> bookings = db.GetBookingList();
            var a = bookings.FindAll(x => x.RoomId == booking.RoomId);
            Check check = new Check();
            if (check.CheckingBooking(booking, a))
            {
                db.Create(booking);
                db.Save();
                return View("OrderResult", new OrderResultModel() { Status = true, Message = "Your order has been succesfully done!" });

            }
            return View("OrderResult", new OrderResultModel() { Status = false, Message = "Time is unavailable" });
        }

        [HttpGet]
        public ActionResult Find()
        {
            return View("Find");
        }

        [HttpPost]
        public ActionResult Find(string keyword)
        {
            return View("SearchResults", db.GetRoomList().FindAll(x => x.RoomNumber.Contains(keyword)));
        }

        [HttpGet]
        public ActionResult PersonalPage(string UserName)
        {
            if (UserName != null)
            {
                PersonalPageModel personalPageModel = new PersonalPageModel();
                using (RoomBookingAuthRepository dbauth = new RoomBookingAuthRepository())
                {
                    var userId = dbauth.GetUserId(UserName);
                    var temp = db.GetBookingList().FindAll(u => u.UserId == userId);
                    foreach (var item in temp)
                    {
                        personalPageModel.BookingList.Add(new Item() { RoomNumber = item.Room.RoomNumber, StartOfSession = item.StartOfSession, EndOfSession = item.EndOfSession });
                    }
                }
                return View("PersonalPage", personalPageModel);
            }
            return View("Index");
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