using System.Collections.Generic;
using System.Linq;
using RectClash.ECS;
using RectClash.ECS.Graphics;
using RectClash.Game;
using RectClash.Game.Combat;
using SFML.Graphics;
using SFML.System;

namespace RectClash.Game
{
	public class CellInfoCom : Com, IGameObv, IDamageInfoCom
	{
		public enum CellType
		{
			Nothing,
			Grass,
			Mud,
			Water,
			Leaf,
			Wood,
			Sand,
			Cactus,
			Snow
		}

		public enum State
		{
			StartingSate,
			UnSelected,
			Selected,
			InMovementRange,
			Attackable,
			CanAttack,
			OnPath,
			TurnComplete
		}

		private class CellTypeInfo
		{
			public Color Background { get; set; }

			public Color BackgroundTop { get; set; }

			public float DamageAdjacent { get; set; }

			public bool BlocksVision { get; set; }

			public bool Selectable { get; set; }

			public CellTypeInfo()
			{
				BackgroundTop = ColorConst.NOTHING;
				Background = ColorConst.NOTHING;
				BlocksVision = false;
				Selectable = true;
			}
		}

		private static Dictionary<State, Color> _fillColorMap = new Dictionary<State, Color>()
		{
			{State.UnSelected, new Color(0, 0, 0, 0)},
			{State.Selected, new Color(0, 0, 125, 120)},
			{State.InMovementRange, new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, 120)},
			{State.OnPath, new Color(0, byte.MaxValue, 0, byte.MaxValue)},
			{State.Attackable, new Color(byte.MaxValue, 0, 0, 170)},
			{State.CanAttack, new Color(byte.MaxValue, 0, 0, 200)},
			{State.TurnComplete, new Color(169, 169, 169, 120)}
		};


		private static Dictionary<CellType, CellTypeInfo> _staticCellInfo = new Dictionary<CellType, CellTypeInfo>()
		{
			{
				CellType.Grass, 
				new CellTypeInfo()
				{
					Background = ColorConst.GRASS
				}
			},
			{
				CellType.Water, 
				new CellTypeInfo()
				{
					Background = ColorConst.WATER, 
					Selectable = false
				}
			},
			{
				CellType.Mud,
				new CellTypeInfo()
				{
					Background = ColorConst.MUD
				}
			},
			{
				CellType.Leaf, 
				new CellTypeInfo()
				{
					Background = ColorConst.LEAF_BASE,
					BackgroundTop = ColorConst.LEAF_OVERLAY
				}
			},
			{
				CellType.Wood, 
				new CellTypeInfo()
				{
					Background = ColorConst.WOOD,
					BackgroundTop = ColorConst.LEAF_OVERLAY,
					Selectable = false,
					BlocksVision = true
				}
			},
			{
				CellType.Sand, 
				new CellTypeInfo()
				{
					Background = ColorConst.SAND
				}
			},
			{
				CellType.Cactus,
				new CellTypeInfo()
				{
					Background = ColorConst.CACTUS,
					DamageAdjacent = 5,
					Selectable = false
				}
			},
			{
				CellType.Snow,
				new CellTypeInfo()
				{
					Background = ColorConst.SNOW
				}
			},
			{
				CellType.Nothing,
				new CellTypeInfo()
			}
		};

		private Stack<State> _stateStack = new Stack<State>();

		private IList<IEnt> _inside = new List<IEnt>();
		
		private DrawRectCom _drawRectCom;

		private CellType _type;

		private DrawRectCom _background;

		private DrawRectCom _backgroundTop;

		public CellType Type 
		{ 
			get => _type;
			set
			{
				_type = value;
				RefreshBackground();
			}
		}

		public Vector2i Cords { get; set; }

		public State CurrentState { get; set; }

		public IEnumerable<IEnt> Inside 
		{ 
			get 
			{ 
				return Owner.Children.Where(i => i.Tags.Contains(Tags.UNIT)); 
			} 
		}

		public DrawRectCom Background 
		{ 
			get => _background;
			set
			{
				_background = value;
				Background.FillColor = _staticCellInfo[Type].Background;
			}
		}

		public DrawRectCom BackgroundTop
		{
			get => _backgroundTop;
			set
			{
				_backgroundTop = value;
				_backgroundTop.FillColor = _staticCellInfo[Type].BackgroundTop;
			}
		}

		public float DamageAdjacent
		{
			get => _staticCellInfo[Type].DamageAdjacent;
		}

		public int MovementCost
		{
			get
			{
				switch(Type)
				{
					case CellType.Mud:
						return 3;
					default:
						return 1;
				}
			}
			
		}

		public bool Selectable 
		{
			get
			{
				return CurrentState != State.TurnComplete && _staticCellInfo[_type].Selectable;
			}
		}

		public bool SpaceAvailable 
		{ 
			get
			{
				return Selectable && Inside.Count() <= 0;
			}
		}

		public double DamageAmount => DamageAdjacent;

		public bool BlocksVision
		{
			get => _staticCellInfo[_type].BlocksVision;
		}

		protected override void InternalStart()
		{
			_drawRectCom = Owner.GetCom<DrawRectCom>();
			ChangeState(State.UnSelected);
		}

		public void ChangeState(State newState)
		{
			if(newState == CurrentState)
				return;

			_stateStack.Push(newState);

			CurrentState = newState;
			_drawRectCom.FillColor = _fillColorMap[newState];
		}

		public void ClearNotNeededChildren()
		{
			if(BackgroundTop.FillColor == new Color(0, 0, 0, 0))
			{
				Engine.Instance.DestoryEnt(BackgroundTop.Owner);
				_backgroundTop = null;
			}
		}
		private void RefreshBackground()
		{
			if(Background != null)
				Background.FillColor = _staticCellInfo[Type].Background;
			if(BackgroundTop != null)
				BackgroundTop.FillColor = _staticCellInfo[Type].BackgroundTop;
		}

		public void OnNotify(IEnt ent, GameEvent evt)
		{
			switch(evt)
			{
				case GameEvent.GRID_CELL_SELECTED:
					Owner.Parent.Notify(Owner, GameEvent.GRID_CELL_SELECTED);
					break;
				case GameEvent.GRID_CLEAR_SELECTION:
					Owner.Parent.Notify(Owner, GameEvent.GRID_CLEAR_SELECTION);
					break;
				case GameEvent.TURN_END:
					ChangeState(CellInfoCom.State.UnSelected);
					break;
				case GameEvent.TURN_START:
					break;
			}
		}

	}
}