using System;
using EventVenueBookingManager.Interfaces;
using EventVenueBookingManager.Models;

namespace EventVenueBookingManager.Patterns
{
    // An observer to write system log to console
    public class ConsoleNotifier : INotificationObserver
    {
        public void Update(BookingEvent bookingEvent)
        {
            Console.WriteLine($"[Notification] {bookingEvent.EventType}: {bookingEvent.Message}");
        }
    }
}
