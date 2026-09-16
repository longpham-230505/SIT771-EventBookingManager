using System.Collections.Generic;
using EventVenueBookingManager.Interfaces;
using EventVenueBookingManager.Models;

namespace EventVenueBookingManager.Services
{
    // Wrapper service layer for conflict checking algorithm
    public class ConflictChecker
    {
        private readonly IConflictStrategy _strategy;

        public ConflictChecker(IConflictStrategy strategy)
        {
            _strategy = strategy;
        }

        public bool HasConflict(Booking candidate, IEnumerable<Booking> existingBookings)
            => _strategy.HasConflict(candidate, existingBookings);
    }
}
