using System.Collections.Generic;

namespace Code.Timer
{
    public static class TimeRemainingExtensions
    {
        private static readonly List<ITimeRemaining> _timeRemainings = new List<ITimeRemaining>(10);

        public static List<ITimeRemaining> TimeRemainings => _timeRemainings;

        public static void AddTimeRemaining(this ITimeRemaining value)
        {
            if (_timeRemainings.Contains(value))
            {
                return;
            }

            value.CurrentTime = value.Time;
            _timeRemainings.Add(value);
        }

        public static void RemoveTimeRemaining(this ITimeRemaining value)
        {
            if (!_timeRemainings.Contains(value))
            {
                return;
            }

            _timeRemainings.Remove(value);
        }
    }
}
