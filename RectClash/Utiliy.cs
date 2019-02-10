using Microsoft.Xna.Framework;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace RectClash
{
	public static class Utiliy
	{
		private static Random _random = new Random();

		public static Random Random { get { return _random; } }

		public static double NextDouble(double minimum, double maximum)
		{
			Random random = new Random();
			return random.NextDouble() * (maximum - minimum) + minimum;
		}

		public static float NextFloat(float minimum, float maximum)
		{
			Random random = new Random();
			return (float)(random.NextDouble() * (maximum - minimum) + minimum);
		}

		public static Vector2f ToSFMLVec(Vector2 vec)
		{
			return new Vector2f(vec.X, vec.Y);
		}

		public static Vector2 ToXNAVec(Vector2f vec)
		{
			return new Vector2(vec.X, vec.Y);
		}

		public static T RandomElement<T>(IList<T> collection)
		{
			return collection[_random.Next(0, collection.Count - 1)];
		}
	}
}
