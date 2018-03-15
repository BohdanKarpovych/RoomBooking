using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoomBooking.Models;

namespace RoomBooking.Repositories
{
    public class RoomBookingAuthRepository : IDisposable
    {
        private RoomBookingContext db;

        public RoomBookingAuthRepository()
        {
            db = new RoomBookingContext();
        }

        public List<User> GetUserList()
        {
            return db.Users.ToList();
        }

        public void Create(User user)
        {
            var userCheck = db.Users.Find(user.Login);
            if (userCheck == null)
            {
                db.Users.Add(user);
            }
            throw new Exception("User with such login already exists");
        }

        public User GetUser(int id)
        {
            return db.Users.Find(id);
        }

        public User GetUser(User user)
        {
            return db.Users.Where(u => u.Login == user.Login && u.Password == user.Password).FirstOrDefault();
        }

        public void Save()
        {
            db.SaveChanges();
        }

        public bool ChangePassword(string newPass, int userId)
        {
            var user = db.Users.Find(userId);
            if (user != null)
            {
                user.Password = newPass;
                return true;
            }
            return false;
        }

        private bool disposed = false;

        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    db.Dispose();
                }
            }
            disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}