using System;
using RectClash.ECS;
using RectClash.ECS.Graphics;
using RectClash.Game.Unit;
using SFML.Graphics;
using RectClash.Game.Perf;
using RectClash.Misc;
using SFML.System;
using System.Collections.Generic;
using UnitType = RectClash.Game.Unit.UnitType;

namespace RectClash.Game
{
	public class EntFactory
	{
		private static readonly EntFactory _instance = new EntFactory();

		private EntFactory() {}

		public static EntFactory Instance { get { return _instance; } }

        public EntFactory(IEnt worldEnt, UnitType unitTypeToCreate, Faction factionToCreate) 
        {
            this.WorldEnt = worldEnt;
                this.UnitTypeToCreate = unitTypeToCreate;
                this.FactionToCreate = factionToCreate;
               
        }
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

			debugEnt.AddCom
			(
				new RenderTextCom()
				{
					Font = font,
					Color = new Color(byte.MaxValue, 0, 0),
					Floating = true,
					Priority = DrawLayer.UI
				}
			);

			Engine.Instance.Window.Camera.Subject.AddObv(debugInfo);
			WorldEnt.GetCom<TurnHandlerCom>().DebugSubject = subject;

			return debugEnt;
		}

		public IEnt CreateWorld()
		{
			var worldEnt = Engine.Instance.CreateEnt();
			var worldCom = worldEnt.AddCom
			(
				new WorldCom()
				{
					WorldSize = new Misc.Vector2<int>(500, 2000)
				}
			);

			WorldEnt =  worldEnt;

			var gridEnt = Engine.Instance.CreateEnt(worldEnt, "Grid");
			var gridCom = gridEnt.AddCom
			(
				new GridCom()
				{
					Subject = new GameSubject()
				}
			);

			var turnHandlerCom = worldEnt.AddCom
			(
				new TurnHandlerCom()
				{
					Subjects = new GameSubject(gridCom)
				}
			);

			gridCom.TurnHandler = turnHandlerCom;

			float scale = 50f;

			var chunksX = 10;
			var chunksY = 2;

			gridCom.GenrateGrid
			(
				chunksX, 
				chunksY,
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
					Subject = new GameSubject(WorldEnt.GetCom<WorldCom>().Grid, WorldEnt.GetCom<TurnHandlerCom>())
				}
			);

			return result;
		}

		private int _footSolider = 0;

		public IEnt CreateFootSolider(GridCom grid, int i, int j)
		{
			var ent = Engine.Instance.CreateEnt
			(
				WorldEnt, 
				"FootSoilder:" + (_footSolider++).ToString() + " " + Utility.GetEnumName(UnitTypeToCreate) + " " + Utility.GetEnumName(FactionToCreate), 
				new List<string>(){Tags.UNIT}
			);

			Color hatColour;

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

			var statusShowEnt = Engine.Instance.CreateEnt(ent);
			statusShowEnt.AddCom
			(
				new DrawRectCom()
				{
					Priority = DrawLayer.UNITS_INFO_TOP
				}
			);

			var unitCom = ent.AddCom
			(
				new UnitInfoCom()
				{
					Type = UnitTypeToCreate,
					Faction = FactionToCreate
				}
			);

			var unitActionCom = ent.AddCom
			(
				new UnitActionContCom()
			);
	
			var progressBarEnt = CreateProgressBar(ent);
			progressBarEnt.PostionCom.LocalScale = new Vector2f(0.9f, 0.4f);
			progressBarEnt.PostionCom.LocalY = -0.2f;
			progressBarEnt.PostionCom.LocalX = 0.1f;

			var progressBarCom = progressBarEnt.GetCom<ProgressBarCom>();

			progressBarCom.Background.FillColor = Color.Red;
			progressBarCom.Forground.FillColor = Color.Green;

			var healthCom = ent.AddCom
			(
				new HealthCom()
				{
					MaxHealth = unitCom.MaxHealth,
					CurrentHealth = unitCom.MaxHealth,
					HealthBarCom = progressBarCom,
					Subject = new GameSubject(unitActionCom)
				}
			);

			ent.AddCom
			(
				new DrawRectCom()
				{
					FillColor = unitCom.BaseColor,
					Priority = DrawLayer.UNITS
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
					Priority = DrawLayer.UNITS_TOP
				}
			);

			grid.AddEnt(ent, i, j);
			return ent;
		}

		public IEnt CreateProgressBar(IEnt ent)
		{
			var resultEnt = Engine.Instance.CreateEnt(ent);
			var backgroundCom = resultEnt.AddCom
			(
				new DrawRectCom()
				{
					Priority = DrawLayer.UNITS_INFO_BOTTOM
				}
			);

			var healthBarForgroundEnt = Engine.Instance.CreateEnt(resultEnt);
			var forgroundCom = healthBarForgroundEnt.AddCom
			(
				new DrawRectCom()
				{
					Priority = DrawLayer.UNITS_INFO_TOP
				}
			);

			resultEnt.AddCom
			(
				new ProgressBarCom()
				{
					Background = backgroundCom,
					Forground = forgroundCom
				}
			);

			return resultEnt;
		}
		

		public IEnt CreateCell(GridCom gridCom, int i, int j, float width, float height)
		{
			var cellName = "Cell:" + i + "," + j;
			var newCell = Engine.Instance.CreateEnt(gridCom.Owner, cellName, Tags.GRID_CELL);

			var infoCom = newCell.AddCom
			(
				new CellInfoCom()
				{
					Cords = new Vector2i(i, j),
					Subject = new GameSubject(gridCom),
					Type = CellInfoCom.CellType.Grass
				}
			);

			newCell.PostionCom.LocalPostion =  new Vector2f(j, i);

			newCell.AddCom
			(
				new DrawRectCom()
				{
					OutlineColor = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue),
					LineThickness = 0.03,
					Priority = DrawLayer.GRID_OVERLAY
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
					Priority = DrawLayer.GRID_BACKGROUND
				}
			);

			infoCom.Background = background;

			return newCell;
		}
	}
}