using System;
using RectClash.ECS;
using RectClash.Game;

namespace RectClash.game
{
    public class EntFactory
    {
        public IEnt CreateWorld()
        {
            var worldEnt = Engine.Instance.CreateEnt();
			var worldCom = worldEnt.AddCom
			(
				new WorldCom()
				{
					WorldSize = new Misc.Vector2<int>(500, 500)
				}
			);

			var gridEnt = Engine.Instance.CreateEnt(worldEnt);
            gridEnt.PostionCom.X += 10;
            gridEnt.PostionCom.Y += 10;
			var gridCom = gridEnt.AddCom
            (
                new GridCom()
            );

            double scale = 70;

			gridCom.GenrateGrid
            (
                (int)(worldCom.WorldSize.X / scale), 
                (int)(worldCom.WorldSize.Y / scale),
                scale,
                scale
            );
			worldCom.Grid = gridCom;

            return worldEnt;
        }
    }
}