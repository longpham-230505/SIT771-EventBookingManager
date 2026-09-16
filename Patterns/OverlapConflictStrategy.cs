using System.Collections.Generic;
using System.Linq;
using EventVenueBookingManager.Interfaces;
using EventVenueBookingManager.Models;

namespace EventVenueBookingManager.Patterns
{
    // A booking conflicts with another if they use the same venue
    // and their time slots overlap.
    public class OverlapConflictStrategy : IConflictStrategy
    {
        public bool HasConflict(Booking candidate, IEnumerable<Booking> existingBookings)
        {
            return existingBookings.Any(b =>
                b.VenueId == candidate.VenueId &&
                b.Id != candidate.Id &&
                b.Status != BookingStatus.Cancelled &&
                b.Slot.Overlaps(candidate.Slot));
        }
    }
}
