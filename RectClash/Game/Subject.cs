using System.Collections.Generic;
using System.Linq;
using RectClash.ECS;

namespace RectClash.Game
{
    public class Subject
    {
    	private ICollection<IGameObv> _obvs = new List<IGameObv>();

        public Subject()
        {
        }

        public Subject(IGameObv obv)
        {
            AddObvs(obv);
        }

        public void AddObvs(IGameObv obv)
        {
            _obvs.Add(obv);
        }

        public void RemoveObvs(IGameObv obv)
        {
            _obvs.Remove(obv);
        }

        public void Notify(ICom com, GameEvent gameEvent)
        {
            Notify(com.Owner, gameEvent);
        }

        public void Notify(IEnt ent, GameEvent gameEvent)
        {
            foreach(var obv in _obvs)
            {
                obv.OnNotify(ent, gameEvent);
            }
        }
    }
}