using RectClash.ECS;

namespace RectClash.Game.Perf
{
    public class PerfMessureCom : Com
    {
        private const long MAXSAMPLES = 100;

		private class MessureTime
		{
			private bool _bufferFilled = false;
			private long _tickindex = 0;
			private long _ticksum = 0;
			private long[] _ticklist = new long[MAXSAMPLES];
			private double _avgTick;

			public double Value
			{
				get => _avgTick;
			}

			private double CalcAverageTick(long newtick)
        	{
				_ticksum -= _ticklist[_tickindex];  /* subtract value falling off */
				_ticksum += newtick;              /* add new value */
				_ticklist[_tickindex] = newtick;   /* save new value so it can be subtracted later */
				if(++_tickindex == MAXSAMPLES)    /* inc buffer index */
				{
					_tickindex = 0;
					_bufferFilled = true;
				}

				/* return average */
				return _bufferFilled ? ((double)_ticksum / MAXSAMPLES) : 0;
        	}

			public void Update(long time)
			{
	            _avgTick = CalcAverageTick(time);
			}

			public override string ToString()
			{
				return Value.ToString("0.##");
			}

		}

		private MessureTime _tick = new MessureTime();
		private MessureTime _drawTime = new MessureTime();
		private MessureTime _updateTime = new MessureTime();

        public override void Update()
        {
			var time = Engine.Instance.Time;

			_tick.Update(time.DeltaTime);
			_drawTime.Update(time.DrawTime);
			_updateTime.Update(time.UpdateTime);

            Owner.Notify("Ticks: " + _tick.ToString(), PerfEvents.TICK_UPDATE);
            Owner.Notify("Update Time: " + _updateTime.ToString(), PerfEvents.UPDATE_TIME);
            Owner.Notify("Draw Time: " + _drawTime.ToString(), PerfEvents.DRAW_TIME_UPDATE);
        }
    }
}