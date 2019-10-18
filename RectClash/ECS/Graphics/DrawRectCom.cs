using SFML.Graphics;

namespace RectClash.ECS.Graphics
{
    public class DrawRectCom: DrawableCom, Drawable
    {
        private RectangleShape _rectangle = new RectangleShape();

        public PostionCom PostionCom { get; set; }

        public Color OutlineColor { get; set; }

        public Color FillColor { get; set; }

        public double LineThickness { get; set; }

        public void Draw(RenderTarget target, RenderStates states)
        {
            // var worldScale = PostionCom.LocalToWorldMatrix.TransformPoint(PostionCom.LocalScale);
            // uint W = (uint)worldScale.X;
            // uint H = (uint)worldScale.Y; // you can change this to full window size later

            // var pixels = new byte[W*H*4];

            // var texture = new Texture(W, H);

            // var sprite = new Sprite(texture); // needed to draw the texture on screen

            // for(int i = 0; i < W*H*4; i += 4) {
            //     pixels[i] = FillColor.R; // obviously, assign the values you need here to form your color
            //     pixels[i+1] = FillColor.G;
            //     pixels[i+2] = FillColor.B;
            //     pixels[i+3] = FillColor.A;
            // }

            // texture.Update(pixels);

            // sprite.Position = PostionCom.WorldPostion;

            _rectangle.Size = new SFML.System.Vector2f(1f, 1f);
            _rectangle.FillColor = FillColor;
            _rectangle.OutlineColor = OutlineColor;
            _rectangle.OutlineThickness = (float)LineThickness;

            states.Transform.Combine(Engine.Instance.Window.Camera.LocalToWorldTransform);
            states.Transform.Combine(PostionCom.LocalToWorldMatrix);

            target.Draw(_rectangle, states);
        }

        protected override void InternalStart()
        {
            PostionCom = Owner.GetCom<PostionCom>();
        }
    }
}