using RoomBooking.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using RoomBooking.Repositories;

namespace RoomBooking.Controllers
{
    public class AccountController : Controller
    {
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (RoomBookingAuthRepository db = new RoomBookingAuthRepository())
                {
                    user = db.GetUser(new User() { Login = model.Login, Password = model.Password });
                }
                if (user != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "User with such login and password does not exist");
                }
            }

            return View(model);
        }

        public ActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                User user = null;
                using (RoomBookingAuthRepository db = new RoomBookingAuthRepository())
                {
                    user = db.GetUser(new User() { Login = model.Login, Password = model.Password });
                }
                if (user == null)
                {
                    using (RoomBookingAuthRepository db = new RoomBookingAuthRepository())
                    {
                        db.Create(new User { Login = model.Login, Password = model.Password });
                        db.Save();
                    }
                    FormsAuthentication.SetAuthCookie(model.Login, true);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("", "User with such login already exists");
            }

            return View(model);
        }
        public ActionResult Logoff()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}