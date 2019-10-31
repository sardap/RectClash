using Priority_Queue;

namespace RectClash.ECS.Graphics
{
    public class DrawableNode : FastPriorityQueueNode
    {
        public IDrawableCom Drawable { get; set; }
    }
}