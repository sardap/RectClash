using SFML.Graphics;

namespace RectClash.ECS.Graphics
{
    public class DrawRectCom: DrawableCom
    {
        public PostionCom PostionCom { get; set; }

        public Color OutlineColor { get; set; }

        public Color FillColor { get; set; }

        public double LineThickness { get; set; }

        protected override void InternalStart()
        {
            PostionCom = Owner.GetCom<PostionCom>();
        }
    }
}