using System.Collections.Generic;
using System.Linq;

namespace RectClash.ECS
{
    public class Subject<S, T>
    {
		private readonly Stack<IObv<S, T>> _newObv = new Stack<IObv<S, T>>();

		private readonly Stack<IObv<S, T>> _removeObv = new Stack<IObv<S, T>>();

    	private readonly ICollection<IObv<S, T>> _obv = new List<IObv<S, T>>();

		private bool _inNotify = false;

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

		/// <summary>
		/// This is a big fucked becuase it can be called recusvily inside of 
		/// the for loop but only the first call should remove and add shit
		/// </summary>
		/// <param name="ent"></param>
		/// <param name="gameEvent"></param>
        public void Notify(S ent, T gameEvent)
        {
			bool first = !_inNotify;

			if(first)
			{
				while(_removeObv.Count > 0)
				{
					_obv.Remove(_removeObv.Pop());
				}

				while(_newObv.Count > 0)
				{
					_obv.Add(_newObv.Pop());
				}

				_inNotify = true;
			}

            foreach(var obv in _obv)
            {
                obv.OnNotify(ent, gameEvent);
            }

			if(first)
			{
				_inNotify = false;
			}
        }
    }
}