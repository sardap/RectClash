using RectClash.ECS;

namespace RectClash.ECS
{
    public interface IObv<S, T>
    {
        void OnNotify(S ent, T evt);
    }
}