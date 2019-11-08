using System.Collections.Generic;
using RectClash.Misc;
using SFML.Graphics;

namespace RectClash.ECS.Graphics
{
    public class DrawRectCom: DrawableCom, Drawable
    {
		private static Vertex[] _sharedVertex = new Vertex[4]
		{
			new Vertex(),
			new Vertex(),
			new Vertex(),
			new Vertex()
		};

		private static Color _noColor = new Color(0 ,0 ,0, 0);

		private static SFML.System.Vector2f _size = new SFML.System.Vector2f(1, 1);

        private RectangleShape _rectangle = new RectangleShape();

        public PostionCom PostionCom { get; set; }

        public Color OutlineColor { get; set; }

        public Color FillColor { get; set; }

        public float LineThickness { get; set; }

		public Vertex[] Vertices 
		{ 
			get
			{
				var transform = Utility.MultiplyTrans(
					Engine.Instance.Window.Camera.LocalToWorldTransform,
					PostionCom.LocalToWorldMatrix
				);

				VertexArray test = new VertexArray();

				var rect = new FloatRect(
					PostionCom.LocalPostion.X,
					PostionCom.LocalPostion.Y, 
					PostionCom.LocalScale.X,
					PostionCom.LocalScale.Y
				);

				var postion = transform.TransformRect(rect);
				_sharedVertex[0].Position = new SFML.System.Vector2f(postion.Left, postion.Top);
				_sharedVertex[1].Position = new SFML.System.Vector2f(postion.Left + postion.Width, postion.Top);
				_sharedVertex[2].Position = new SFML.System.Vector2f(postion.Left + postion.Width, postion.Top + postion.Height);
				_sharedVertex[3].Position = new SFML.System.Vector2f(postion.Left, postion.Top + postion.Height);

				_sharedVertex[0].Color = FillColor;
				_sharedVertex[1].Color = FillColor;
				_sharedVertex[2].Color = FillColor;
				_sharedVertex[3].Color = FillColor;

				

				return _sharedVertex;
			}
		}

        public void Draw(RenderTarget target, RenderStates states)
        {          
            _rectangle.FillColor = FillColor;
            _rectangle.OutlineColor = OutlineColor;
            _rectangle.OutlineThickness = LineThickness;

            states.Transform.Combine(Engine.Instance.Window.Camera.LocalToWorldTransform);
            states.Transform.Combine(PostionCom.LocalToWorldMatrix);

			_rectangle.Transform.Combine(states.Transform);

			target.Draw(_rectangle, states);
        }

        protected override void InternalStart()
        {
            PostionCom = Owner.GetCom<PostionCom>();
			_rectangle.Size = _size;
        }
    }
}