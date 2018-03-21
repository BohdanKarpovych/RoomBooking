using System;
using System.Collections.Generic;
using System.Linq;
using RoomBooking.ServiceClasses;
using RoomBooking.Models;

namespace RoomBooking.Repositories
{
    public class RoomBookingAuthRepository : IDisposable
    {
        private RoomBookingContext db;
        private MD5HashCalculating md5;

        public RoomBookingAuthRepository()
        {
            db = new RoomBookingContext();
            md5 = new MD5HashCalculating();
        }

        public List<User> GetUserList()
        {
            return db.Users.ToList();
        }

        public void Create(User user)
        {
            var userCheck = db.Users.Where(u => user.Login == u.Login).FirstOrDefault();
            if (userCheck == null)
            {
                db.Users.Add(user);
            }
            else
            {
                throw new Exception("User with such login already exists");
            }
        }

        public User GetUser(int id)
        {
            return db.Users.Find(id);
        }

        public User GetUser(User user)
        {
            return db.Users.Where(u => u.Login == user.Login).FirstOrDefault();
        }

        public int GetUserId(string name)
        {
            return db.Users.Where(u => u.Login == name).First().UserId;
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
                user.Password = md5.CalculateMD5Hash(newPass);
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