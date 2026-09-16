namespace EventVenueBookingManager.Models
{
    public enum VenueType
    {
        ConferenceHall,
        CommunityHall,
        OutdoorSpace
    }

    public enum EventType
    {
        Conference,
        Wedding,
        Workshop,
        Concert,
        Other
    }

    public enum UserRole
    {
        Admin,
        Organizer,
        Customer
    }

    public enum BookingStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }
}
