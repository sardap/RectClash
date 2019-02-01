using Microsoft.Xna.Framework;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace RectClash
{
	public static class Utiliy
	{
		public static Vector2f ToSFMLVec(Vector2 vec)
		{
			return new Vector2f(vec.X, vec.Y);
		}

		public static Vector2 ToXNAVec(Vector2f vec)
		{
			return new Vector2(vec.X, vec.Y);
		}
	}
}
