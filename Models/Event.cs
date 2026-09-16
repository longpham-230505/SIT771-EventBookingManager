using System;
using EventVenueBookingManager.Common;

namespace EventVenueBookingManager.Models
{
    public class Event : IEntity
    {
        public string Id { get; }
        public string Name { get; set; }
        public EventType Type { get; set; }
        public DateTime StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int ExpectedAttendance { get; set; }
        public string VenueId { get; set; }

        public Event(string id, string name, EventType type, DateTime startTime,
            TimeSpan duration, int expectedAttendance, string venueId)
        {
            Id = id;
            Name = name;
            Type = type;
            StartTime = startTime;
            Duration = duration;
            ExpectedAttendance = expectedAttendance;
            VenueId = venueId;
        }

        public TimeSlot ToTimeSlot() => new TimeSlot(StartTime, StartTime + Duration);

        public override string ToString() => $"{Name} ({Type}) at {StartTime:g}";
    }
}
