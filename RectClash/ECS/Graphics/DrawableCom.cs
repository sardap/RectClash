namespace RectClash.ECS.Graphics
{
    public class DrawableCom : Com, IDrawableCom
    {
        public bool Floating { get; set; }

        public int Priority { get ; set ; }
    }
}