using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoomBooking.Models;

namespace RoomBooking.Repositories
{
    interface IRoomBookingRepository : IDisposable
    {
        List<Room> GetRoomList();
        Room GetRoom(int id);
        List<Booking> GetBookingList();
        Booking GetBooking(int id);
      
        void Create(Room room);
        void Create(Booking booking);

        bool Update(Room room);
        bool Update(Booking booking);

        bool Delete(Room room);
        bool Delete(Booking booking);

        void Save();
    }
}
