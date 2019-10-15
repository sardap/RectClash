using RectClash.ECS;

namespace RectClash.Game
{
    public interface IGameObv
    {
        void OnNotify(IEnt ent, GameEvent evt);
    }
}