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
			Cactus
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
			public Color Background { get; private set; }

			public Color BackgroundTop { get; private set; }

			public float DamageAdjacent { get; private set; }

			public CellTypeInfo(Color background, Color backgroundTop, float damageAdjacent = 0)
			{
				Background = background;
				BackgroundTop = backgroundTop;
				DamageAdjacent = damageAdjacent;
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
				new CellTypeInfo(new Color(109, 168, 74), new Color(0, 0, 0, 0))
			},
			{
				CellType.Water, 
				new CellTypeInfo(new Color(0, 0, 255), new Color(0, 0, 0, 0))
			},
			{
				CellType.Mud, 
				new CellTypeInfo(new Color(143, 116, 63), new Color(0, 0, 0, 0))
			},
			{
				CellType.Leaf, 
				new CellTypeInfo(new Color(109, 168, 74), new Color(82, 107, 45, 200))
			},
			{
				CellType.Wood, 
				new CellTypeInfo(new Color(193, 154, 107), new Color(82, 107, 45, 200))
			},
			{
				CellType.Sand, 
				new CellTypeInfo(new Color(238, 214, 175), new Color(0, 0, 0, 0))
			},
			{
				CellType.Cactus, 
				new CellTypeInfo(new Color(91, 111, 85), new Color(0, 0, 0, 0), 5)
			},
			{
				CellType.Nothing,
				new CellTypeInfo(new Color(0,0,0,0), new Color(0,0,0,0))
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

		public GameSubject Subject { get; set; }

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
				return (Type == CellType.Grass || Type == CellType.Mud || Type == CellType.Leaf || Type == CellType.Sand) && CurrentState != State.TurnComplete;
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

		public void OnNotify(IEnt ent, GameEvent evt)
		{
			switch(evt)
			{
				case GameEvent.GRID_CELL_SELECTED:
					Subject.Notify(Owner, GameEvent.GRID_CELL_SELECTED);
					break;
				case GameEvent.GRID_CLEAR_SELECTION:
					Subject.Notify(Owner, GameEvent.GRID_CLEAR_SELECTION);
					break;
			}
		}

		private void RefreshBackground()
		{
			if(Background != null)
				Background.FillColor = _staticCellInfo[Type].Background;
			if(BackgroundTop != null)
				BackgroundTop.FillColor = _staticCellInfo[Type].BackgroundTop;
		}
	}
}