using System;
using System.Collections.Generic;
using System.Linq;
using RectClash.ECS;
using SFML.Graphics;

namespace RectClash.Misc
{
    public static class Utility
    {
        private static Random _random = new Random();
        
		public static Random Random { get { return _random; } }

		public static bool RandomBool()
		{
			return Random.NextDouble() >= 0.5;
		}

		public static int RandomInt(int minValue = int.MinValue, int maxValue = int.MaxValue)
		{
			return Random.Next(minValue, maxValue);
		}

		public static double RandomDouble(double minValue = 0.0, double maxValue = 1.0)
		{
			return Random.Next((int)minValue, (int)maxValue) + Random.NextDouble();
		}

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

		public static T RandomElement<T>(IEnumerable<T> enumerable)
		{
		    return enumerable.ElementAt(Random.Next(0, enumerable.Count()));
		}

		public static T RandomElement<T>(IList<T> collection)
		{
			return collection[_random.Next(0, collection.Count)];
		}

        public static bool WorldMouseInRect(double x, double y, double width, double height)
        {
            return PointInRect(Engine.Instance.Mouse.WorldMouseX, Engine.Instance.Mouse.WorldMouseY, x, y, width, height);
        }

        public static bool PointInRect(double x1, double y1, double x2, double y2, double width, double height)
        {
            return 
                x1 <= x2 + width && 
                x1 >= x2 &&
                y1 <= y2 + height &&
                y1 >= y2;

        }

        private static Transform MultiplyTransRec(int i, params Transform[] trans)
		{
			if(i >= trans.Length)
			{
				return trans[i - 1];
			}

			return trans[i] * MultiplyTransRec(i + 1, trans);
		}

		public static Transform MultiplyTrans(params Transform[] trans)
		{
			return MultiplyTransRec(0, trans);
		}

        public static string GetEnumName<T>(T val) where T : struct, IConvertible 
        {
            return Enum.GetName(val.GetType(), val);
        }

		public static T RandomEnum<T>() where T : struct, IConvertible
		{
			return RandomElement(Enum.GetValues(typeof(T)).Cast<T>());
		}

		public static List<Tuple<int, int>> GetAdjacentSquares<T>(int i, int j, T[,] multiAry)
		{
			var result = new List<Tuple<int, int>>();

			if(i + 1 < multiAry.GetLength(0))
			{
				result.Add(new Tuple<int, int>(i + 1, j));
			}

			if(i - 1 >= 0)
			{
				result.Add(new Tuple<int, int>(i - 1, j));
			}

			if(j + 1 < multiAry.GetLength(1))
			{
				result.Add(new Tuple<int, int>(i, j + 1));
			}

			if(j - 1 >= 0)
			{
				result.Add(new Tuple<int, int>(i, j - 1));
			}

			return result;
		}

		public static List<Tuple<int, int>> GetAdjacentSquaresSixDirections<T>(int i, int j, T[,] multiAry)
		{
			var result = GetAdjacentSquares(i, j, multiAry);

			if(j - 1 >= 0 && i + 1 < multiAry.GetLength(0))
			{
				result.Add(new Tuple<int, int>(i + 1, j - 1));
			}

			if(j + 1 < multiAry.GetLength(1) && i + 1 < multiAry.GetLength(0))
			{
				result.Add(new Tuple<int, int>(i + 1, j + 1));
			}

			if(j + 1 < multiAry.GetLength(1) && i - 1 >= 0)
			{
				result.Add(new Tuple<int, int>(i - 1, j + 1));
			}

			if(j - 1 >= 0 && i - 1 >= 0)
			{
				result.Add(new Tuple<int, int>(i - 1, j - 1));
			}

			return result;
		}

    }
}