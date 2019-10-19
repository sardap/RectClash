using System;
using RectClash.ECS;
using RectClash.ECS.Graphics;
using RectClash.Game.Unit;
using RectClash.Game;
using RectClash.Game.Unit;
using SFML.Graphics;
using UnitInfoCom = RectClash.Game.Unit.UnitInfoCom;
using UnitType = RectClash.Game.Unit.UnitType;
using RectClash.Game.Perf;
using RectClash.Misc;
using SFML.System;

namespace RectClash.Game
{
    public class EntFactory
    {
        private static readonly EntFactory _instance = new EntFactory();

        private EntFactory() {}

        public static EntFactory Instance { get { return _instance; } }

        public IEnt WorldEnt { get; set; }

        public UnitType UnitTypeToCreate { get; set; }

        public Faction FactionToCreate { get; set; }

        public IEnt CreateDebugInfo()
        {
			var font = new ECS.Graphics.Font()
			{
				FileLocation = "calibri.ttf"
			};

            var debugEnt = Engine.Instance.CreateEnt();
			debugEnt.PostionCom.LocalX = 10;
			debugEnt.PostionCom.LocalY = 10;


            var debugInfo = debugEnt.AddCom
            (
                new UpdateDebugInfoCom()
            );

            var subject = new Subject<string, PerfEvents>(debugInfo);
            
            debugEnt.AddCom
            (
                new PerfMessureCom()
                {
                    Subject = subject
                }
                
            );

            debugEnt.AddCom
            (
                new DebugInputCom()
                {
                    Subject = subject
                }
                
            );

            Engine.Instance.Window.Camera.Subject.AddObvs(debugInfo);

			debugEnt.AddCom
			(
				new RenderTextCom()
				{
					Font = font,
					Color = new Color(byte.MaxValue, 0, 0),
					Floating = true,
                    Priority = DrawPriority.UI
				}
			);

            return debugEnt;
        }

        public IEnt CreateWorld()
        {
            var worldEnt = Engine.Instance.CreateEnt();
			var worldCom = worldEnt.AddCom
			(
				new WorldCom()
				{
					WorldSize = new Misc.Vector2<int>(500, 1000)
				}
			);

            WorldEnt =  worldEnt;

			var gridEnt = Engine.Instance.CreateEnt(worldEnt, "Grid");
			var gridCom = gridEnt.AddCom
            (
                new GridCom()
            );

            float scale = 50f;

			gridCom.GenrateGrid
            (
                (int)(worldCom.WorldSize.X / scale), 
                (int)(worldCom.WorldSize.Y / scale),
                10,
                10
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
					Subject = new GameSubject(WorldEnt.GetCom<WorldCom>().Grid)
				}
			);

            return result;
        }

        public IEnt CreateFootSolider(GridCom grid, int i, int j)
        {
            var ent = Engine.Instance.CreateEnt(WorldEnt);
            grid.AddEnt(ent, i, j);           
            Color baseColour;
            Color hatColour;
            int range;
            switch(UnitTypeToCreate)
            {
                case UnitType.Regular:
                    baseColour = Color.White;
                    range = 5;
                    break;
                case UnitType.Heavy:
                    baseColour = Color.Magenta;
                    range = 3;
                    break;
                default:
                    throw new System.NotImplementedException();
            }

            switch(FactionToCreate)
            {
                case Faction.Red:
                    hatColour = Color.Red;
                    break;
                case Faction.Blue:
                    hatColour = Color.Blue;
                    break;
                default:
                    throw new System.NotImplementedException();
            }

            ent.AddCom
            (
                new UnitInfoCom()
                {
                    Range = range,
                    Type = UnitTypeToCreate,
                    Faction = FactionToCreate
                }
            );

            ent.AddCom
            (
                new DrawRectCom()
                {
                    FillColor = baseColour,
                    Priority = DrawPriority.UNITS
                }
            );

            var hat = Engine.Instance.CreateEnt(ent);
            hat.PostionCom.LocalScale = new Vector2f(0.8f, 0.8f);
            hat.PostionCom.LocalPostion = new Vector2f(0.2f, 0.2f);
            hat.AddCom
            (
                new DrawRectCom()
                {
                    OutlineColor = hatColour,
                    LineThickness = 0.1,
                    Priority = DrawPriority.UNITS_TOP_LAYER
                }
            );
            
            return ent;
        }

        public IEnt CreateCell(GridCom gridCom, int i, int j, float width, float height)
        {
            var cellName = "Cell:" + i + "," + j;
            var newCell = Engine.Instance.CreateEnt(gridCom.Owner, cellName);

            var cellType = CellInfoCom.CellType.Dirt;
            var selectorNum = Utility.Random.Next(100);
            if(selectorNum > 30)
            {
                cellType = CellInfoCom.CellType.Dirt;
            }
            else if (selectorNum > 10)
            {
                cellType = CellInfoCom.CellType.Mud;
            }
            else
            {
                cellType = CellInfoCom.CellType.Water;
            }

            var infoCom = newCell.AddCom
            (
                new CellInfoCom()
                {
                    Cords = new Vector2i(i, j),
                    Subject = new GameSubject(gridCom),
                    Type = cellType
                }
            );

            newCell.PostionCom.LocalPostion =  new Vector2f(j, i);

            newCell.AddCom
            (
                new DrawRectCom()
                {
                    OutlineColor = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue),
                    LineThickness = 0.03,
                    Priority = DrawPriority.GRID_OVERLAY
                }
            );
            
            newCell.AddCom
            (
                new CellInputCom()
            );

            var cellBackgroundEnt = Engine.Instance.CreateEnt(newCell, "Background: " + cellName);
            var background = cellBackgroundEnt.AddCom
            (
                new DrawRectCom()
                {
                    Priority = DrawPriority.GRID_BACKGROUND
                }
            );

            infoCom.Background = background;

            return newCell;
        }
    }
}