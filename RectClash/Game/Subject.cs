using System.Collections.Generic;
using System.Linq;
using RectClash.ECS;

namespace RectClash.Game
{
    public class Subject
    {
    	private ICollection<IObv> _obvs = new List<IObv>();

        public void AddObvs(IObv obv)
        {
            _obvs.Add(obv);
        }

        public void RemoveObvs(IObv obv)
        {
            _obvs.Remove(obv);
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