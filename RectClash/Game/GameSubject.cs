using RectClash.ECS;
using System.Collections.Generic;

namespace RectClash.Game
{
    public class GameSubject : Subject<IEnt, GameEvent>
    {
        public GameSubject()
        {
        }

        public GameSubject(IGameObv obv) : base(obv)
        {
        }

        public GameSubject(params IGameObv[] obvs) : base()
		{
			foreach(var obv in obvs)
			{
				AddObv(obv);
			}
		}

    }
}