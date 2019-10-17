using RectClash.ECS;

namespace RectClash.Game.Perf
{
    public class PerfMessureCom : Com
    {
        private const long MAXSAMPLES = 100;
        private long tickindex = 0;
        private long ticksum = 0;
        private long[] ticklist = new long[MAXSAMPLES];

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
            ticksum -= ticklist[tickindex];  /* subtract value falling off */
            ticksum += newtick;              /* add new value */
            ticklist[tickindex] = newtick;   /* save new value so it can be subtracted later */
            if(++tickindex == MAXSAMPLES)    /* inc buffer index */
                tickindex = 0;

            /* return average */
            return((double)ticksum / MAXSAMPLES);
        }
    }
}