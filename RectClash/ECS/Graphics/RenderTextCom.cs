using RectClash.Misc;
using SFML.Graphics;

namespace RectClash.ECS.Graphics
{
    public class RenderTextCom : DrawableCom
    {
        public PostionCom Postion { get; set; }

        public string Text { get; set; }

        public Font Font { get; set; }

        public Color Color { get; set; }

        protected override void InternalStart()
        {
            Postion = Owner.GetCom<PostionCom>();
        }
    }
}