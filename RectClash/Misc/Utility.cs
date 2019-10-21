using System;
using System.Collections.Generic;
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

		public static double RandomDouble(double minValue = double.MinValue, double maxValue = double.MaxValue)
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

		public static T RandomElement<T>(IList<T> collection)
		{
			return collection[_random.Next(0, collection.Count - 1)];
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
    }
}