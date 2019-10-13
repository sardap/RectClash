namespace RectClash.ECS.Graphics
{
    public class DrawRectCom: DrawableCom
    {
        public PostionCom PostionCom { get; set; }
        
        public double Width { get; set; }

        public double Height { get; set; }

        public Colour OutlineColour { get; set; }

        public double LineThickness { get; set; }

        protected override void InternalStart()
        {
            PostionCom = Owner.GetCom<PostionCom>();
        }
    }
}