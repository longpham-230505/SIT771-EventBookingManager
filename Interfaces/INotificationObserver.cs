using EventVenueBookingManager.Models;

namespace EventVenueBookingManager.Interfaces
{
    // Observer pattern for notification system
    public interface INotificationObserver
    {
        void Update(BookingEvent bookingEvent);
    }
}
