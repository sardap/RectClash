using System;
using System.Diagnostics;

namespace RectClash.ECS
{
    public class Time
    {
        private Stopwatch _stopWatch = new Stopwatch();
        private long _timeSinceStart;
        private long _oldTimeStart;
        private long _deltaTime;

        public long DeltaTime
        {
            get
            {
                return _deltaTime;
            }
        }

        public double DeltaTimePercentOfSec
        {
            get
            {
                return _deltaTime / 1000.0;
            }
        }

        public long ElapsedTime
        {
            get
            {
                return _stopWatch.ElapsedMilliseconds;
            }
        }

        public void Start()
        {
            _stopWatch.Start();
        }

        public void StartOfLoop()
        {
            _deltaTime = ElapsedTime - _oldTimeStart;
            _oldTimeStart = ElapsedTime;
        }
    }
}