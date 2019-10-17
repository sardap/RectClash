using RectClash.ECS;
using RectClash.ECS.Graphics;

namespace RectClash.Game.Unit
{
    public class UnitInfoCom : Com
    {
        public int Range { get; set; }

        public UnitType Type { get; set; }

        public Faction Faction { get; set; }
    }
}