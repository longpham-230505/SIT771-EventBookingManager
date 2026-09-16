using EventVenueBookingManager.Common;

namespace EventVenueBookingManager.Models
{
    public class Booking : IEntity
    {
        public string Id { get; }
        public string EventId { get; }
        public string VenueId { get; }
        public string OrganizerId { get; }
        public TimeSlot Slot { get; }
        public BookingStatus Status { get; private set; }

        public Booking(string id, string eventId, string venueId, string organizerId, TimeSlot slot)
        {
            Id = id;
            EventId = eventId;
            VenueId = venueId;
            OrganizerId = organizerId;
            Slot = slot;
            Status = BookingStatus.Pending;
        }

        public void Confirm() => Status = BookingStatus.Confirmed;
        public void Cancel() => Status = BookingStatus.Cancelled;

        public override string ToString() => $"Booking {Id} [{Status}] {Slot}";
    }
}
