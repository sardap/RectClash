using System.Collections.Generic;
using System.Linq;

namespace RectClash.ECS
{
    public class Subject<S, T>
    {
    	private ICollection<IObv<S, T>> _obvs = new List<IObv<S, T>>();

        public Subject()
        {
        }

        public Subject(IObv<S, T> obv)
        {
            AddObvs(obv);
        }

        public void AddObvs(IObv<S, T> obv)
        {
            _obvs.Add(obv);
        }

        public void RemoveObvs(IObv<S, T> obv)
        {
            _obvs.Remove(obv);
        }

        public void Notify(S ent, T gameEvent)
        {
            foreach(var obv in _obvs)
            {
                obv.OnNotify(ent, gameEvent);
            }
        }
    }
}