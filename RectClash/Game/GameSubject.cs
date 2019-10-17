using RectClash.ECS;

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
    }
}