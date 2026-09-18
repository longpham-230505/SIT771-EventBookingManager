using System.Collections.Generic;
using EventVenueBookingManager.Common;

namespace EventVenueBookingManager.Models
{
    // Abstract parent class
    public abstract class Venue : IEntity
    {
        public string Id { get; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public abstract VenueType Type { get; }

        protected Venue(string id, string name, int capacity)
        {
            Id = id;
            Name = name;
            Capacity = capacity;
        }

        public virtual bool IsAvailable(TimeSlot slot, IEnumerable<Booking> existingBookings)
        {
            foreach (var booking in existingBookings)
            {
                if (booking.VenueId == Id &&
                    booking.Status != BookingStatus.Cancelled &&
                    booking.Slot.Overlaps(slot))
                {
                    return false;
                }
            }
            return true;
        }

        public override string ToString() => $"{Name} [{Type}] (capacity {Capacity})";
    }

    public class ConferenceHall : Venue
    {
        public override VenueType Type => VenueType.ConferenceHall;
        public ConferenceHall(string id, string name, int capacity) : base(id, name, capacity) { }
    }

    public class CommunityHall : Venue
    {
        public override VenueType Type => VenueType.CommunityHall;
        public CommunityHall(string id, string name, int capacity) : base(id, name, capacity) { }
    }

    public class OutdoorSpace : Venue
    {
        public override VenueType Type => VenueType.OutdoorSpace;
        public OutdoorSpace(string id, string name, int capacity) : base(id, name, capacity) { }
    }
}
