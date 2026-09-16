using System;
using EventVenueBookingManager.Interfaces;
using EventVenueBookingManager.Models;

namespace EventVenueBookingManager.Patterns
{
    // Factory pattern for venue creation.
    public class VenueFactory : IVenueFactory
    {
        public Venue CreateVenue(VenueType type, string id, string name, int capacity)
        {
            return type switch
            {
                VenueType.ConferenceHall => new ConferenceHall(id, name, capacity),
                VenueType.CommunityHall => new CommunityHall(id, name, capacity),
                VenueType.OutdoorSpace => new OutdoorSpace(id, name, capacity),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown venue type.")
            };
        }
    }
}
