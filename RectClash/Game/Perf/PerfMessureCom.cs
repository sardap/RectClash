using RectClash.ECS;

namespace RectClash.Game.Perf
{
    public class PerfMessureCom : Com
    {
        private const long MAXSAMPLES = 100;
		private bool _bufferFilled = false;
        private long _tickindex = 0;
        private long _ticksum = 0;
        private long[] _ticklist = new long[MAXSAMPLES];

        private double _avgTick;

        public double AvgTick { get { return _avgTick; } }

        public Subject<string, PerfEvents> Subject { get; set; }

        public override void Update()
        {
            _avgTick = CalcAverageTick(Engine.Instance.Time.DeltaTime);
            Subject.Notify("Ticks: " + _avgTick.ToString("0.##"), PerfEvents.TICK_UPDATE);
        }

        /* need to zero out the ticklist array before starting */
        /* average will ramp up until the buffer is full */
        /* returns average ticks per frame over the MAXSAMPLES last frames */

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
    }
}