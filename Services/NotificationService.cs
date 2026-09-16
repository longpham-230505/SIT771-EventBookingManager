using System.Collections.Generic;
using EventVenueBookingManager.Interfaces;
using EventVenueBookingManager.Models;

namespace EventVenueBookingManager.Services
{
    // Listener service for observer objects
    public class NotificationService
    {
        private readonly List<INotificationObserver> _observers = new();

        public void Subscribe(INotificationObserver observer) => _observers.Add(observer);

        public void Unsubscribe(INotificationObserver observer) => _observers.Remove(observer);

        public void Notify(BookingEvent bookingEvent)
        {
            foreach (var observer in _observers)
            {
                observer.Update(bookingEvent);
            }
        }
    }
}
