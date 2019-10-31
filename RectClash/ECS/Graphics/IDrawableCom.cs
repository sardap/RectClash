namespace RectClash.ECS.Graphics
{
    public interface IDrawableCom : ICom
    {
        bool Floating { get; set; }

        DrawLayer Priority { get; set; }

		DrawableNode DrawableNode { get; }
    }
}