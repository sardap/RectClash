using RectClash.Misc;
using SFML.Graphics;

namespace RectClash.ECS.Graphics
{
    public class DrawCircleCom : DrawableCom
    {
        public PostionCom PostionCom;

        public double Radius { get; set; }
        public Color Color { get; set; }

        protected override void InternalStart()
        {
            PostionCom = Owner.GetCom<PostionCom>();
        }
    }
}