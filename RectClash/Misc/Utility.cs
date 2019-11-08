using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RectClash.ECS;
using SFML.Graphics;
using SFML.System;

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

		private static ulong Rand(ulong seed)
		{
			var temp = (seed * 1210057048);
			temp /= 1064531262;
			temp += 162069246;
			temp %= 152890752;
			temp -= 199921687;
			var result = ((temp % long.MaxValue) + 1);
			return result;
		}

		public static long Randomlong(long minValue, long maxValue, long seed)
		{	
			var randomNum = (long)Rand((ulong)seed);
			var result = minValue + randomNum % (maxValue - minValue);
			Debug.Assert(result >= minValue && result < maxValue);
			return result;
		}

		public static long Randomlong(long seed)
		{
			return Randomlong(0, long.MaxValue, seed);
		}


		public static int RandomInt(int minValue, int maxValue, long seed)
		{
			return (int)Randomlong(minValue, maxValue, seed);
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

		public static T RandomElement<T>(IEnumerable<T> enumerable, long seed)
		{
			var index = RandomInt(0, enumerable.Count(), seed);
			return enumerable.ElementAt(index);
		}


		public static T RandomElement<T>(IEnumerable<T> enumerable)
		{
		    return enumerable.ElementAt(Random.Next(0, enumerable.Count()));
		}

		public static T RandomElement<T>(T[,] array)
		{
			int values = array.GetLength(0) * array.GetLength(1);
			int index = Random.Next(values);
			return array[index / array.GetLength(0), index % array.GetLength(0)];
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

		public static List<Vector2i> GetAdjacentSquares<T>(int i, int j, T[,] multiAry)
		{
			var result = new List<Vector2i>();

			if(i + 1 < multiAry.GetLength(0))
			{
				result.Add(new Vector2i(i + 1, j));
			}

			if(i - 1 >= 0)
			{
				result.Add(new Vector2i(i - 1, j));
			}

			if(j + 1 < multiAry.GetLength(1))
			{
				result.Add(new Vector2i(i, j + 1));
			}

			if(j - 1 >= 0)
			{
				result.Add(new Vector2i(i, j - 1));
			}

			return result;
		}

		public static List<Vector2i> GetAdjacentSquaresSixDirections<T>(int i, int j, T[,] multiAry)
		{
			var result = GetAdjacentSquares(i, j, multiAry);

			if(j - 1 >= 0 && i + 1 < multiAry.GetLength(0))
			{
				result.Add(new Vector2i(i + 1, j - 1));
			}

			if(j + 1 < multiAry.GetLength(1) && i + 1 < multiAry.GetLength(0))
			{
				result.Add(new Vector2i(i + 1, j + 1));
			}

			if(j + 1 < multiAry.GetLength(1) && i - 1 >= 0)
			{
				result.Add(new Vector2i(i - 1, j + 1));
			}

			if(j - 1 >= 0 && i - 1 >= 0)
			{
				result.Add(new Vector2i(i - 1, j - 1));
			}

			return result;
		}

		public static double DistanceBetween(Vector2i aPos, Vector2i bPos)
		{
			var dx = System.Math.Abs(aPos.X - bPos.X);
			var dy = System.Math.Abs(aPos.Y - bPos.Y);

			return dx + dy;
		}

		public static void AddAll<T>(ICollection<T> collection, IEnumerable<T> toAdd)
		{
			foreach(var i in toAdd)
			{
				collection.Add(i);
			}
		}
    }
}