using System;
using RectClash.ECS;
using RectClash.ECS.Graphics;
using RectClash.ECS.Performance;
using RectClash.Game;
using SFML.Graphics;

namespace RectClash.game
{
    public class EntFactory
    {
        private static readonly EntFactory _instance = new EntFactory();

        private EntFactory() {}

        public static EntFactory Instance { get { return _instance; } }

        public IEnt WorldEnt { get; set; }

        public IEnt CreateDebugInfo()
        {
			var font = new ECS.Graphics.Font()
			{
				FileLocation = "calibri.ttf"
			};

            var debugEnt = Engine.Instance.CreateEnt();
			debugEnt.PostionCom.X = 10;
			debugEnt.PostionCom.Y = 10;
			debugEnt.AddCom
			(
				new RenderTextCom()
				{
					Font = font,
					Color = new Color(byte.MaxValue, 0, 0),
					Floating = true,
                    Priority = int.MaxValue
				}
			);
			debugEnt.AddCom(new UpdateDebugInfoCom());

            return debugEnt;
        }

        public IEnt CreateWorld()
        {
            var worldEnt = Engine.Instance.CreateEnt();
			var worldCom = worldEnt.AddCom
			(
				new WorldCom()
				{
					WorldSize = new Misc.Vector2<int>(1000, 1000)
				}
			);

            WorldEnt =  worldEnt;

			var gridEnt = Engine.Instance.CreateEnt(worldEnt);
            gridEnt.PostionCom.X += 10;
            gridEnt.PostionCom.Y += 10;
			var gridCom = gridEnt.AddCom
            (
                new GridCom()
            );

            float scale = 50f;

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

        public IEnt CreatePlayerInput()
        {
            var result = Engine.Instance.CreateEnt();
            result.AddCom
			(
				new PlayerInputCom()
				{
					Subject = new Subject(WorldEnt.GetCom<WorldCom>().Grid)
				}
			);

            return result;
        }

        public IEnt CreateFootSolider(GridCom grid, int i, int j)
        {
            var ent = Engine.Instance.CreateEnt(WorldEnt);
            grid.AddEnt(ent, i, j);
            ent.AddCom
            (
                new DrawRectCom()
                {
                    FillColor = Color.White,
                    Priority = 5
                }
            );
            ent.AddCom
            (
                new UnitInfoCom()
                {
                    Range = 5
                }
            );
            
            return ent;
        }
    }
}