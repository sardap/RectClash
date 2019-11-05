using System.Collections.Generic;
using SFML.Graphics;

namespace RectClash.ECS.Graphics
{
    public class DrawRectCom: DrawableCom, Drawable
    {
		private static Color _noColor = new Color(0 ,0 ,0, 0);

		private static SFML.System.Vector2f _size = new SFML.System.Vector2f(1, 1);

        private RectangleShape _rectangle = new RectangleShape();

        public PostionCom PostionCom { get; set; }

        public Color OutlineColor { get; set; }

        public Color FillColor { get; set; }

        public float LineThickness { get; set; }

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