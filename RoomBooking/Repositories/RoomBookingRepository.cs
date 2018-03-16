using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using RoomBooking.Models;
using System.Data.Entity;

namespace RoomBooking.Repositories
{
    public class RoomBookingRepository : IRoomBookingRepository
    {
        private RoomBookingContext db;

        public RoomBookingRepository()
        {
            db = new RoomBookingContext();
        }
        public List<Room> GetRoomList()
        {
            return db.Rooms.ToList();
        }

        public Room GetRoom(int id)
        {
            return db.Rooms.Find(id);
        }

        public List<Booking> GetBookingList()
        {
            return db.Bookings.ToList();
        }

        public Booking GetBooking(int id)
        {
            return db.Bookings.Find(id);
        }

        public void Create(Room room)
        {
            db.Rooms.Add(room);
        }
        public void Create(Booking booking)
        {
            //var duration = Math.Abs(booking.StartOfSession - booking.EndOfSession);
            //if (duration >= 30 && duration % 30 == 0)
            //{
                db.Bookings.Add(booking);
            //}
            //else
            //{
            //    throw new Exception("Incorrect duration");
            //}
        }

        public bool Update(Room room)
        {
            db.Entry(room).State = EntityState.Modified;
            return true;
        }

        public bool Update(Booking booking)
        {
            db.Entry(booking).State = EntityState.Modified;
            return true;
        }

        public bool Delete(Room room)
        {
            db.Rooms.Remove(room);
            return true;
        }

        public bool Delete(Booking booking)
        {
            db.Bookings.Remove(booking);
            return true;
        }

        public void Save()
        {
            db.SaveChanges();
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