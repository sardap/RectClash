using RectClash.ECS;

namespace RectClash.Game
{
    public interface IObv
    {
        void OnNotify(IEnt ent, GameEvent evt);
    }
}