using RoomBooking.Models;
using System.Web.Mvc;
using System.Web.Security;
using RoomBooking.Repositories;
using RoomBooking.ServiceClasses;
using System.Linq;
using System.Data.SqlClient;

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
            RoomBookingContext a = new RoomBookingContext();

            var result = a.Users.SqlQuery($"SELECT * FROM dbo.Users WHERE Login = '{model.Login}' AND Password = '{model.Password}'").ToList();

            //var login = new SqlParameter("@login", model.Login);
            //var pass = new SqlParameter("@pass", model.Password);
            //var result = a.Users.SqlQuery($"SELECT * FROM dbo.Users WHERE Login = @login AND Password = @pass", login, pass).ToList();

            if (result.Count != 0)
            {
                FormsAuthentication.SetAuthCookie(result.FirstOrDefault().Login, true);
                return RedirectToAction("Index", "Home");
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