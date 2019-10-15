namespace RectClash.ECS.Graphics
{
    public interface IDrawableCom : ICom
    {
        bool Floating { get; set; }

        int Priority { get; set; }
    }
}