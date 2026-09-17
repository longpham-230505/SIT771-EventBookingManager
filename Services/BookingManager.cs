using System;
using EventVenueBookingManager.Interfaces;
using EventVenueBookingManager.Models;

namespace EventVenueBookingManager.Services
{
    public class BookingManager
    {
        private readonly IRepository<Venue> _venueRepository;
        private readonly IRepository<Event> _eventRepository;
        private readonly IRepository<Booking> _bookingRepository;
        private readonly IVenueFactory _venueFactory;
        private readonly ConflictChecker _conflictChecker;
        private readonly NotificationService _notificationService;

        public BookingManager(
            IRepository<Venue> venueRepository,
            IRepository<Event> eventRepository,
            IRepository<Booking> bookingRepository,
            IVenueFactory venueFactory,
            ConflictChecker conflictChecker,
            NotificationService notificationService)
        {
            _venueRepository = venueRepository;
            _eventRepository = eventRepository;
            _bookingRepository = bookingRepository;
            _venueFactory = venueFactory;
            _conflictChecker = conflictChecker;
            _notificationService = notificationService;
        }

        public Venue AddVenue(VenueType type, string id, string name, int capacity)
        {
            var venue = _venueFactory.CreateVenue(type, id, name, capacity);
            _venueRepository.Add(venue);
            return venue;
        }

        public Booking CreateBooking(Event @event, string venueId, string organizerId)
        {
            _eventRepository.Add(@event);

            var slot = @event.ToTimeSlot();
            var booking = new Booking(
                id: Guid.NewGuid().ToString("N"),
                eventId: @event.Id,
                venueId: venueId,
                organizerId: organizerId,
                slot: slot);

            if (_conflictChecker.HasConflict(booking, _bookingRepository.GetAll()))
            {
                _notificationService.Notify(new BookingEvent(
                    booking,
                    BookingEventType.Cancelled,
                    $"Booking rejected: venue {venueId} is already booked for that time slot."));

                throw new InvalidOperationException("Booking conflicts with an existing booking for this venue.");
            }

            booking.Confirm();
            _bookingRepository.Add(booking);

            _notificationService.Notify(new BookingEvent(
                booking,
                BookingEventType.Created,
                $"Booking {booking.Id} confirmed for event '{@event.Name}'."));

            return booking;
        }

        public void CancelBooking(string bookingId)
        {
            var booking = _bookingRepository.FindById(bookingId)
                ?? throw new InvalidOperationException($"Booking {bookingId} not found.");

            booking.Cancel();

            _notificationService.Notify(new BookingEvent(
                booking,
                BookingEventType.Cancelled,
                $"Booking {booking.Id} has been cancelled."));
        }
    }
}
