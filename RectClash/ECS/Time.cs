using System;
using System.Diagnostics;

namespace RectClash.ECS
{
    public class Time
    {
        private Stopwatch _stopWatch = new Stopwatch();
        private long _oldTimeStart;
        private long _deltaTime;

		private long _drawTimeStart;

		private long _updateTimeStart;

        public long DeltaTime
        {
            get
            {
                return _deltaTime;
            }
        }

        public float DeltaTimePercentOfSec
        {
            get
            {
                return _deltaTime / 1000.0f;
            }
        }

        public long ElapsedTime
        {
            get
            {
                return _stopWatch.ElapsedMilliseconds;
            }
        }

		public long DrawTime
		{
			get;
			private set;
		}

		public long UpdateTime
		{
			get;
			private set;
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

		public void StartOfDraw()
		{
			_drawTimeStart = ElapsedTime;
		}

		public void EndOfDraw()
		{
			DrawTime = ElapsedTime - _drawTimeStart;
		}

		public void StartOfUpdate()
		{
			_updateTimeStart = ElapsedTime;
		}

		public void EndOfUpdate()
		{
			UpdateTime = ElapsedTime - _updateTimeStart;
		}
    }
}