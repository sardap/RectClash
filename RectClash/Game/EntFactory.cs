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
using RectClash.Game.AI;

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
			debugEnt.Subject = new DebugSubject();
			debugEnt.PostionCom.LocalX = 10;
			debugEnt.PostionCom.LocalY = 10;


			var debugCom = debugEnt.AddCom(new UpdateDebugInfoCom());
			((DebugSubject)debugEnt.Subject).AddObv(debugCom);

			debugEnt.AddCom
			(
				new PerfMessureCom()
			);

			debugEnt.AddCom
			(
				new DebugInputCom()
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

			Engine.Instance.Window.Camera.Subject.AddObv(debugCom);
			WorldEnt.GetCom<TurnHandlerCom>().DebugSubject = (DebugSubject)debugEnt.Subject;

			return debugEnt;
		}

		public IEnt CreateWorld()
		{
			var worldEnt = Engine.Instance.CreateEnt();
			worldEnt.Subject = new GameSubject();
			var worldCom = worldEnt.AddCom
			(
				new WorldCom()
				{
					WorldSize = new Misc.Vector2<int>(500, 2000)
				}
			);

			WorldEnt =  worldEnt;

			var gridEnt = Engine.Instance.CreateEnt(worldEnt, "Grid");
			gridEnt.Subject = new GameSubject();
			var gridCom = gridEnt.AddCom
			(
				new GridCom(){}
			);
			((GameSubject)gridEnt.Subject).AddObv(gridCom);

			var turnHandlerCom = worldEnt.AddCom
			(
				new TurnHandlerCom()
				{
					GridEnt = gridEnt
				}
			);
			((GameSubject)worldEnt.Subject).AddObv(turnHandlerCom);

			gridCom.TurnHandler = turnHandlerCom;

			var chunksX = GameConstants.CHUNKS_X;
			var chunksY = GameConstants.CHUNKS_Y;

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
					TurnHandlerEnt = WorldEnt,
					GridEnt = WorldEnt.GetCom<WorldCom>().Grid.Owner
				}
			);

			return result;
		}

		private int _footSolider = 0;

		public IEnt CreateFootSolider(GridCom grid, int i, int j)
		{
			return CreateFootSolider(grid, i, j, FactionToCreate, UnitTypeToCreate);
		}


		public IEnt CreateFootSolider(GridCom grid, int i, int j, Faction factionToCreate, UnitType unitTypeToCreate)
		{
			var ent = Engine.Instance.CreateEnt
			(
				WorldEnt, 
				"FootSoilder:" + (_footSolider++).ToString() + " " + Utility.GetEnumName(unitTypeToCreate) + " " + Utility.GetEnumName(factionToCreate), 
				new List<string>(){Tags.UNIT}
			);

			Color hatColour;

			switch(factionToCreate)
			{
				case Faction.Red:
					hatColour = Color.Red;
					break;
				case Faction.Blue:
					hatColour = Color.Blue;
					break;
				case Faction.Green:
					hatColour = Color.Green;
					break;
				default:
					throw new System.NotImplementedException();
			}

			var statusShowEnt = Engine.Instance.CreateEnt(ent);
			ent.Subject = new GameSubject();

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
					Type = unitTypeToCreate,
					Faction = factionToCreate
				}
			);
			((GameSubject)ent.Subject).AddObv(unitCom);

			var unitActionCom = ent.AddCom
			(
				new UnitActionContCom()
			);
			((GameSubject)ent.Subject).AddObv(unitActionCom);

			if(factionToCreate != WorldEnt.GetCom<WorldCom>().PlayerFaction)
			{
				var regAiCom = ent.AddCom(new RegularSoliderAICom());
				((GameSubject)ent.Subject).AddObv(regAiCom);
			}
	
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
					HealthBarCom = progressBarCom
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
					LineThickness = 0.1f,
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
			newCell.Subject = new GameSubject();

			var infoCom = newCell.AddCom
			(
				new CellInfoCom()
				{
					Cords = new Vector2i(i, j),
					Type = CellInfoCom.CellType.Nothing
				}
			);
			
			((GameSubject)newCell.Subject).AddObv(infoCom);

			newCell.PostionCom.LocalPostion =  new Vector2f(j, i);

			newCell.AddCom
			(
				new DrawRectCom()
				{
					OutlineColor = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue),
					LineThickness = 0,
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

			var cellBackgroundTopEnt = Engine.Instance.CreateEnt(newCell, "BackgroundTop: " + cellName);
			var backgroundTop = cellBackgroundTopEnt.AddCom
			(
				new DrawRectCom()
				{
					Priority = DrawLayer.GRID_BACKGROUND_TOP
				}
			);

			infoCom.Background = background;
			infoCom.BackgroundTop = backgroundTop;

			

			return newCell;
		}
	}
}