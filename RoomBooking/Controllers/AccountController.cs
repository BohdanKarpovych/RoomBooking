using RoomBooking.Models;
using System.Web.Mvc;
using System.Web.Security;
using RoomBooking.Repositories;
using RoomBooking.ServiceClasses;

namespace RoomBooking.Controllers
{
    public class AccountController : Controller
    {
        MD5HashCalculating md5;

        public AccountController()
        {
            md5 = new MD5HashCalculating();
        }

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
                    user = db.GetUser(new User() { Login = model.Login, Password = md5.CalculateMD5Hash(model.Password) });
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
            //System.Text.RegularExpressions.Regex a = new System.Text.RegularExpressions.Regex(@"[a - z]{8,}");
            //System.Text.RegularExpressions.MatchCollection m = a.Matches(model.Password);
            if (ModelState.IsValid)
            {
                User user = null;
                using (RoomBookingAuthRepository db = new RoomBookingAuthRepository())
                {
                    user = db.GetUser(new User() { Login = model.Login, Password = md5.CalculateMD5Hash(model.Password) });
                }
                if (user == null)
                {
                    using (RoomBookingAuthRepository db = new RoomBookingAuthRepository())
                    {
                        db.Create(new User { Login = model.Login, Password = md5.CalculateMD5Hash(model.Password) });
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