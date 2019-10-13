using RectClash.Misc;

namespace RectClash.ECS.Graphics
{
    public class DrawCircleCom : DrawableCom
    {
        public PostionCom PostionCom;

        public double Radius { get; set; }
        public Colour Colour { get; set; }

        protected override void InternalStart()
        {
            PostionCom = Owner.GetCom<PostionCom>();
        }
    }
}