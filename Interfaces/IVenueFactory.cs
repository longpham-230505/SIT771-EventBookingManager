using EventVenueBookingManager.Models;

namespace EventVenueBookingManager.Interfaces
{
    public interface IVenueFactory
    {
        Venue CreateVenue(VenueType type, string id, string name, int capacity);
    }
}
