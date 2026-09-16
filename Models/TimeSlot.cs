using System;

namespace EventVenueBookingManager.Models
{
    // Small value type representing a start/end window
    public class TimeSlot
    {
        public DateTime Start { get; }
        public DateTime End { get; }

        public TimeSlot(DateTime start, DateTime end)
        {
            if (end <= start)
                throw new ArgumentException("End time must be after start time.");

            Start = start;
            End = end;
        }

        public bool Overlaps(TimeSlot other) => Start < other.End && other.Start < End;

        public override string ToString() => $"{Start:g} - {End:g}";
    }
}
