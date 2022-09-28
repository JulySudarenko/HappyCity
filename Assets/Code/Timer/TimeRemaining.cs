using System;

namespace Code.Timer
{
    public class TimeRemaining : ITimeRemaining
    {
        public Action Method { get; }
        public bool IsRepeating { get; }
        public float Time { get; }
        public float CurrentTime { get; set; }

        public TimeRemaining(Action method, float time, bool isRepeating = false)
        {
            Method = method;
            Time = time;
            CurrentTime = time;
            IsRepeating = isRepeating;
        }
    }
}
