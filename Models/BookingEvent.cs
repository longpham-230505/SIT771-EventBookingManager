namespace EventVenueBookingManager.Models
{
    // Simple class to represent what happened
    // to a booking, not the booking's own state.
    public enum BookingEventType
    {
        Created,
        Confirmed,
        Cancelled,
        Modified
    }

    public class BookingEvent
    {
        public Booking Booking { get; }
        public BookingEventType EventType { get; }
        public string Message { get; }

        public BookingEvent(Booking booking, BookingEventType eventType, string message)
        {
            Booking = booking;
            EventType = eventType;
            Message = message;
        }
    }
}
