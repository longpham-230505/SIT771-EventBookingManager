using System;
using EventVenueBookingManager.Interfaces;
using EventVenueBookingManager.Models;

namespace EventVenueBookingManager.Patterns
{
    // An observer mimicking the automatic email feature
    // In a real system this would call an email service instead of printing.
    public class EmailNotifier : INotificationObserver
    {
        public void Update(BookingEvent bookingEvent)
        {
            Console.WriteLine($"[Email stub] Would email organizer about: {bookingEvent.Message}");
        }
    }
}
