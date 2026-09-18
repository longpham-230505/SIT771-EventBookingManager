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
        // Sequence variables to keep track of events and venues
        private static int _venueSeq = 1;
        private static int _eventSeq = 1;

        public static void Main(string[] args)
        {
            var venueRepository = new VenueRepository();
            var eventRepository = new EventRepository();
            var bookingRepository = new BookingRepository();
            var userRepository = new UserRepository();

            IVenueFactory venueFactory = new VenueFactory();
            IConflictStrategy conflictStrategy = new OverlapConflictStrategy();
            var conflictChecker = new ConflictChecker(conflictStrategy);

            var notificationService = new NotificationService();
            notificationService.Subscribe(new ConsoleNotifier());
            notificationService.Subscribe(new EmailNotifier());

            var bookingManager = new BookingManager(venueRepository, eventRepository, bookingRepository,
                venueFactory, conflictChecker, notificationService);

            SeedSampleVenues(bookingManager);

            // Welcome text when starting the system
            Console.WriteLine("Welcome to the Event & Venue Booking Manager");

            // Looping to mimic the log-in / log-out behavior
            while (true)
            {
                // Log-in to the current session
                var currentUser = LogIn(userRepository);

                // Start the loop
                var logOut = RunMenuLoop(currentUser, bookingManager, venueRepository, eventRepository, bookingRepository);

                // Exit the loop when option 'Quit' is picked
                if (!logOut) break;
            }

            Console.WriteLine();
            Console.WriteLine("Goodbye!");
        }

        // Function to seed the database with sample data
        private static void SeedSampleVenues(BookingManager bookingManager)
        {
            bookingManager.AddVenue(VenueType.ConferenceHall, NextVenueId(), "Main Conference Hall", 150);
            bookingManager.AddVenue(VenueType.CommunityHall, NextVenueId(), "Riverside Community Hall", 80);
        }

        // Function to automate the venue and event ID generation
        private static string NextVenueId() => $"V{_venueSeq++:D3}";
        private static string NextEventId() => $"E{_eventSeq++:D3}";

        private static User LogIn(UserRepository userRepository)
        {
            // Asks for credential
            Console.WriteLine();
            var name = Helper.ReadNonEmptyString("Your name");
            var email = Helper.ReadNonEmptyString("Your email");
            var role = Helper.ChooseEnum<UserRole>("Select your role:");

            // Create the user instance
            User user = role switch
            {
                UserRole.Admin => new Admin(Guid.NewGuid().ToString("N"), name, email),
                UserRole.Organizer => new Organizer(Guid.NewGuid().ToString("N"), name, email),
                _ => new Customer(Guid.NewGuid().ToString("N"), name, email)
            };

            // Add user instance to database
            userRepository.Add(user);
            Console.WriteLine($"Welcome, {user}!");
            return user;
        }

        private static bool RunMenuLoop(User currentUser, BookingManager bookingManager, VenueRepository venueRepository,
            EventRepository eventRepository, BookingRepository bookingRepository)
        {
            // Each entry is protected by the same permission check the domain model already defines (User.CanPerform)
            var menuItems = new List<(string Label, string PermissionAction, Action Handler)>
            {
                ("Browse venues", "BrowseVenues", () => BrowseVenues(venueRepository)),
                ("Add a venue", "AddVenue", () => AddVenue(bookingManager)),
                ("Create event & booking", "CreateBooking", () => CreateEventAndBooking(bookingManager, venueRepository, currentUser)),
                ("View events", "ViewEvents", () => ViewEvents(eventRepository)),
                ("Cancel a booking", "CancelOwnBooking", () => CancelBooking(bookingManager, bookingRepository, currentUser)),
            };

            while (true)
            {
                // Lists all action the user has permission to perform
                var available = menuItems.Where(m => currentUser.CanPerform(m.PermissionAction)).ToList();

                // Asks for an option
                Console.WriteLine();
                Console.WriteLine("What would you like to do?");
                for (int i = 0; i < available.Count; i++)
                    Console.WriteLine($"  {i + 1}. {available[i].Label}");

                // Option for logging out
                Console.WriteLine($"  {available.Count + 1}. Log out");

                // O is 'Quit' by default
                Console.WriteLine("  0. Quit");

                var choice = Helper.ReadInt("Choose an option");

                // Default options to terminate the current log-in session
                if (choice == 0) return false;
                if (choice == available.Count + 1) return true;

                // Validate the chosen option
                if (choice < 1 || choice > available.Count)
                {
                    Console.WriteLine("Invalid choice, try again.");
                    continue;
                }

                try
                {  
                    // Performs the chosen option
                    available[choice - 1].Handler();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }

        private static void BrowseVenues(VenueRepository venueRepository)
        {
            // Gets all venues from database
            var venues = venueRepository.GetAll().ToList();
            Console.WriteLine();
            if (venues.Count == 0)
            {
                Console.WriteLine("No venues have been added yet.");
                return;
            }

            // Prints out all venues
            Console.WriteLine("Venues:");
            foreach (var venue in venues)
            {
                Console.WriteLine($"  - {venue}");
            }
        }

        private static void AddVenue(BookingManager bookingManager)
        {
            // Asks for venue information
            Console.WriteLine();
            var name = Helper.ReadNonEmptyString("Venue name");
            var capacity = Helper.ReadInt("Capacity");
            var type = Helper.ChooseEnum<VenueType>("Venue type:");

            // Creates and adds the venue
            var venue = bookingManager.AddVenue(type, NextVenueId(), name, capacity);
            Console.WriteLine($"Added venue: {venue}");
        }

        private static void CreateEventAndBooking(BookingManager bookingManager, VenueRepository venueRepository, User currentUser)
        {
            Console.WriteLine();
            var venues = venueRepository.GetAll().ToList();
            var venue = Helper.ChooseFromList("Select a venue for this event:", venues);
            if (venue is null)
                return;

            // Asks for booking details
            var name = Helper.ReadNonEmptyString("Event name");
            var type = Helper.ChooseEnum<EventType>("Event type:");
            var start = Helper.ReadDateTime("Event start");
            var durationHours = Helper.ReadDouble("Duration (hours)");
            var attendance = Helper.ReadInt("Expected attendance");

            // Creates new event and booking based on the details
            var newEvent = new Event(
                NextEventId(), name, type, start, TimeSpan.FromHours(durationHours), attendance, venue.Id);

            var booking = bookingManager.CreateBooking(newEvent, venue.Id, currentUser.Id);
            Console.WriteLine($"Booking confirmed: {booking}");
        }

        private static void ViewEvents(EventRepository eventRepository)
        {
            var events = eventRepository.GetAll().ToList();

            // If no active event is available
            Console.WriteLine();
            if (events.Count == 0)
            {
                Console.WriteLine("No events have been created yet.");
                return;
            }

            // Prints out all active events
            Console.WriteLine("Events:");
            foreach (var e in events)
                Console.WriteLine($"  - {e} (expected attendance: {e.ExpectedAttendance})");
        }

        private static void CancelBooking(BookingManager bookingManager, BookingRepository bookingRepository, User currentUser)
        {
            // Admins can cancel any booking
            // Organizers can cancel only their own bookings
            // Customers don't have permission
            var candidates = bookingRepository.GetAll()
                .Where(b => b.Status != BookingStatus.Cancelled)
                .Where(b => currentUser is Admin || b.OrganizerId == currentUser.Id)
                .ToList();

            Console.WriteLine();
            var booking = Helper.ChooseFromList("Select a booking to cancel:", candidates);
            if (booking is null)
                return;
            
            // Cancel the selected booking
            bookingManager.CancelBooking(booking.Id);
            Console.WriteLine($"Cancelled booking {booking.Id}.");
        }
    }
}
