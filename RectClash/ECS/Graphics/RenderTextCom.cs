using RectClash.Misc;

namespace RectClash.ECS.Graphics
{
    public class RenderTextCom : DrawableCom
    {
        public PostionCom Postion { get; set; }

        public string Text { get; set; }

        public Font Font { get; set; }

        public Colour Colour { get; set; }

        protected override void InternalStart()
        {
            Postion = Owner.GetCom<PostionCom>();
        }
    }
}