namespace RectClash.ECS.Graphics
{
    public interface IDrawableCom : ICom
    {
        bool Floating { get; set; }

        DrawPriority Priority { get; set; }
    }
}