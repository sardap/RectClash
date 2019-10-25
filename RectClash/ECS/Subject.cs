using System.Collections.Generic;
using System.Linq;

namespace RectClash.ECS
{
    public class Subject<S, T>
    {
		private readonly Stack<IObv<S, T>> _newObv = new Stack<IObv<S, T>>();
		private readonly Stack<IObv<S, T>> _removeObv = new Stack<IObv<S, T>>();

    	private readonly ICollection<IObv<S, T>> _obv = new List<IObv<S, T>>();

        public Subject()
        {
        }

        public Subject(IObv<S, T> obv)
        {
            AddObv(obv);
        }

        public void AddObv(IObv<S, T> obv)
        {
			_newObv.Push(obv);
        }

        public void RemoveObv(IObv<S, T> obv)
        {
			if(!_obv.Contains(obv) && !_newObv.Contains(obv))
			{
				throw new System.InvalidOperationException();
			}

			_removeObv.Push(obv);
        }

        public void Notify(S ent, T gameEvent)
        {
			while(_removeObv.Count > 0)
			{
				_obv.Remove(_removeObv.Pop());
			}

			while(_newObv.Count > 0)
			{
				_obv.Add(_newObv.Pop());
			}

            foreach(var obv in _obv)
            {
                obv.OnNotify(ent, gameEvent);
            }
        }
    }
}