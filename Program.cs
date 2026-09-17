using System;
using EventVenueBookingManager.Interfaces;
using EventVenueBookingManager.Models;
using EventVenueBookingManager.Patterns;
using EventVenueBookingManager.Repositories;
using EventVenueBookingManager.Services;

namespace EventVenueBookingManager
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            IRepository<Venue> venueRepository = new VenueRepository();
            IRepository<Event> eventRepository = new EventRepository();
            IRepository<Booking> bookingRepository = new BookingRepository();
            IRepository<User> userRepository = new UserRepository();

            IVenueFactory venueFactory = new VenueFactory();
            IConflictStrategy conflictStrategy = new OverlapConflictStrategy();
            var conflictChecker = new ConflictChecker(conflictStrategy);

            var notificationService = new NotificationService();
            notificationService.Subscribe(new ConsoleNotifier());
            notificationService.Subscribe(new EmailNotifier());

            var bookingManager = new BookingManager(
                venueRepository, eventRepository, bookingRepository,
                venueFactory, conflictChecker, notificationService);


            var venue = bookingManager.AddVenue(VenueType.ConferenceHall, "V001", "Main Conference Hall", 150);
            Console.WriteLine($"Created venue: {venue}");

            var organizer = new Organizer("U001", "Long Pham", "longpham@example.com");
            userRepository.Add(organizer);

            var sampleEvent = new Event(
                id: "E001",
                name: "Quarterly Strategy Meeting",
                type: EventType.Conference,
                startTime: DateTime.Today.AddDays(1).AddHours(9),
                duration: TimeSpan.FromHours(2),
                expectedAttendance: 80,
                venueId: venue.Id);

            var booking = bookingManager.CreateBooking(sampleEvent, venue.Id, organizer.Id);
            Console.WriteLine($"Created booking: {booking}");
        }
    }
}
