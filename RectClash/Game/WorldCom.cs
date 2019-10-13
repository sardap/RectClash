using System;
using System.Collections.Generic;
using RectClash.ECS;
using RectClash.Misc;

namespace RectClash.Game
{
    public class WorldCom: Com
    {
        public GridCom Grid { get; set; }

        public Vector2<int> WorldSize { get; set; }

        public List<UnitInfoCom> Units { get; set; }

        public WorldCom() : base()
        {
            Units = new List<UnitInfoCom>();
        }

        protected override void InternalStart()
        {
            Units = new List<UnitInfoCom>();
        }

        public override void Update()
        {
            foreach(var i in Units)
            {

            }
        }
    }
}