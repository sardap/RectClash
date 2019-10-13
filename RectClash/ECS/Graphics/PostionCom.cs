using RectClash.Misc;
using SFML.Graphics;
using SFML.Graphics.Glsl;

namespace RectClash.ECS.Graphics
{
	// Stolen https://www.gamedev.net/articles/programming/math-and-physics/making-a-game-engine-transformations-r3566/
	public class PostionCom : Com
	{
		private Vector2<float> _localPos = new Vector2<float>(0f, 0f);

		public double X { get; set; }

		public double Y { get; set; }

		public PostionCom()
		{
		}
	}
}