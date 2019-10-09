using System.Diagnostics;

namespace RectClash.ECS
{
    public class Time
    {
        private Stopwatch _stopWatch = new Stopwatch();
        private long _startTime;
        private long _deltaTime;

        public long DeltaTime
        {
            get
            {
                return _stopWatch.ElapsedMilliseconds - _startTime;
            }
        }

        public void StartOfLoop()
        {
            _stopWatch.Start();
            _startTime = _stopWatch.ElapsedMilliseconds;
        }
    }
}