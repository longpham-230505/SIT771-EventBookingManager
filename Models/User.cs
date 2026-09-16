using System.Collections.Generic;
using EventVenueBookingManager.Common;

namespace EventVenueBookingManager.Models
{
    // Abstract parent class
    public abstract class User : IEntity
    {
        public string Id { get; }
        public string Name { get; set; }
        public string Email { get; set; }
        public UserRole Role { get; protected set; }

        protected User(string id, string name, string email)
        {
            Id = id;
            Name = name;
            Email = email;
        }

        // Each role decides for itself which actions it can perform.
        public abstract bool CanPerform(string action);

        public override string ToString() => $"{Name} ({Role})";
    }

    public class Admin : User
    {
        public Admin(string id, string name, string email) : base(id, name, email)
        {
            Role = UserRole.Admin;
        }

        // Admins can do anything 
        public override bool CanPerform(string action) => true;
    }

    public class Organizer : User
    {
        private static readonly HashSet<string> AllowedActions = new()
        {
            "CreateEvent", "CreateBooking", "CancelOwnBooking", "ViewReports"
        };

        public Organizer(string id, string name, string email) : base(id, name, email)
        {
            Role = UserRole.Organizer;
        }

        public override bool CanPerform(string action) => AllowedActions.Contains(action);
    }

    public class Customer : User
    {
        private static readonly HashSet<string> AllowedActions = new()
        {
            "BrowseVenues", "ViewEvents"
        };

        public Customer(string id, string name, string email) : base(id, name, email)
        {
            Role = UserRole.Customer;
        }

        public override bool CanPerform(string action) => AllowedActions.Contains(action);
    }
}
