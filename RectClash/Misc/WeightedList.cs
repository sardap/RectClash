using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace RectClash.Misc
{
	public class WeightValue<T>
	{
		public int Weight { get; private set; }
		public T Value { get; private set; }

		public WeightValue(int weight, T value)
		{
			Weight = weight;
			Value = value;
		}
	}


    public class WeightedList<T> : IEnumerable<WeightValue<T>>
    {
        private List<WeightValue<T>> _list;

		private int WeightSum
		{
			get
			{
				int result = 0;

				foreach(var i in _list)
				{
					result += i.Weight;
				}

				return result;
			}
		}

		public List<WeightValue<T>> InternalList
		{
			get => _list;
		}

		public WeightedList(params WeightValue<T>[] elements)
		{
			_list = new List<WeightValue<T>>();

			_list.AddRange(elements);
		}

		public WeightedList() : this(new WeightValue<T>[0])
		{

		}
		
		public void Add(int weight, T element)
		{
			_list.Add(new WeightValue<T>(weight, element));
		}

		public T RandomValue(long seed)
		{
			var randomVal = Utility.RandomInt(0, WeightSum, seed);

			var sum = _list.Sum(i => i.Weight);

			for(int i = 0; i < _list.Count - 1; i++)
			{
				sum -= _list[i].Weight;

				if(randomVal > sum)
				{
					return _list[i].Value;
				}

			}

			return _list[_list.Count - 1].Value;
		}

		public IEnumerator<WeightValue<T>> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}
	}
}