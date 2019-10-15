using SFML.Graphics;

namespace RectClash.ECS.Graphics
{
	// Stolen https://www.gamedev.net/articles/programming/math-and-physics/making-a-game-engine-transformations-r3566/
	public class PostionCom : Com
	{
		private Transform _trans = Transform.Identity;

		public SFML.System.Vector2f _postion = new SFML.System.Vector2f();

		public SFML.System.Vector2f _size = new SFML.System.Vector2f();

		public SFML.System.Vector2f Postion
		{
			get => _postion;
			set => _postion = value;
		}

		public SFML.System.Vector2f Size
		{
			get => _size;
			set => _size = value;
		}

		public Transform Transform
		{ 
			get => _trans;
		}

		public float SizeX
		{
			get => Size.X;
			set => Size = new SFML.System.Vector2f(value, SizeY);
		}

		public float SizeY
		{
			get => Size.Y;
			set => Size = new SFML.System.Vector2f(SizeX, value);
		}

		public float X
		{
			get => Postion.X;
			set => Postion = new SFML.System.Vector2f(value, Y);
		}

		public float Y
		{
			get => Postion.Y;
			set => Postion = new SFML.System.Vector2f(X, value);
		}

		public FloatRect Rect
		{
			get => new FloatRect(X, Y, SizeX, SizeY);
		}

		public PostionCom()
		{
		}
	}
}