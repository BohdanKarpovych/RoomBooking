using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace RoomBooking.Models
{
    public class RoomDbInitializer : DropCreateDatabaseAlways<RoomBookingContext>
    {
        protected override void Seed(RoomBookingContext db)
        {
            db.Rooms.Add(new Room { RoomNumber = "605" });
            db.Rooms.Add(new Room { RoomNumber = "606" });
            db.Rooms.Add(new Room { RoomNumber = "607" });
            db.Rooms.Add(new Room { RoomNumber = "608" });
            db.Rooms.Add(new Room { RoomNumber = "610" });

            db.Persons.Add(new Person { FirstName = "John", LastName = "A" });
            db.Persons.Add(new Person { FirstName = "Will", LastName = "B" });
            db.Persons.Add(new Person { FirstName = "Ann", LastName = "C" });

            db.Bookings.Add(new Booking { PersonId = 1, RoomId = 1, StartOfSession = new DateTime(2018, 3, 9, 15, 30, 0), EndOfSession = new DateTime(2018, 3, 9, 16, 30, 0) });

            base.Seed(db);
        }
    }
}