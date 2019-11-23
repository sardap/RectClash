using RectClash.ECS;
using System.Collections.Generic;
using RectClash.ECS.Graphics;
using SFML.Graphics;
using RectClash.Game.Combat;
using System.Linq;
using RectClash.Misc;

namespace RectClash.Game.Unit
{
	public class UnitInfoCom : Com, IDamageInfoCom, IGameObv
	{
		private class CommonUnitInfo
		{
			public double maxHealth;
			public double damage;
			public Color baseColor;
			public int movementRange;
			public int attackRange;
			public DifficultyRating difficultyRating;
			public UnitSoundInfo SoundInfo;

		}

		private static Dictionary<UnitType, CommonUnitInfo> _staticUnitInfo = new Dictionary<UnitType, CommonUnitInfo>()
		{
			{
				UnitType.Regular,
				new CommonUnitInfo()
				{
					damage = 10.0d,
					baseColor = Color.White,
					movementRange = 5,
					attackRange = GameConstants.MELEE_ATTACK_RANGE,
					maxHealth = 30d,
					difficultyRating = DifficultyRating.Low,
					SoundInfo = new UnitSoundInfo()
					{
						DeathSound = GameConstants.SOUND_FOOT_SOLIDER_DIED,
						SelectSound = GameConstants.SOUND_FOOT_SOLIDER_SELECTED,
						MoveSound = GameConstants.SOUND_FOOT_SOLIDER_MOVE,
						AttackSound = GameConstants.SOUND_FOOT_SOLIDER_ATTACK,
						DamageSound = GameConstants.SOUND_DAMAGE_SOUND_ALL
					}
				}
			},
			{
				UnitType.Heavy,
				new CommonUnitInfo()
				{
					damage = 20.0d,
					baseColor = Color.Magenta,
					movementRange = 3,
					attackRange = GameConstants.MELEE_ATTACK_RANGE,
					maxHealth = 60d,
					difficultyRating = DifficultyRating.High,
					SoundInfo = new UnitSoundInfo()
					{
						DeathSound = GameConstants.SOUND_HEAVY_SOLIDER_DIED,
						SelectSound = GameConstants.SOUND_HEAVY_SOLIDER_SELECTED,
						MoveSound = GameConstants.SOUND_HEAVY_SOLIDER_MOVE,
						AttackSound = GameConstants.SOUND_HEAVY_SOLIDER_ATTACK,
						DamageSound = GameConstants.SOUND_DAMAGE_SOUND_ALL
					}
				}
			},
			{
				UnitType.LightArcher,
				new CommonUnitInfo()
				{
					damage = 5.0d,
					baseColor = new Color(255, 255, 102),
					movementRange = 4,
					attackRange = 4,
					maxHealth = 10d,
					difficultyRating = DifficultyRating.Low,
					SoundInfo = new UnitSoundInfo()
					{
						DeathSound = GameConstants.SOUND_LIGHT_ARCHER_DIED,
						SelectSound = GameConstants.SOUND_LIGHT_ARCHER_SELECTED,
						MoveSound = GameConstants.SOUND_LIGHT_ARCHER_MOVE,
						AttackSound = GameConstants.SOUND_LIGHT_ARCHER_ATTACK,
						DamageSound = GameConstants.SOUND_DAMAGE_SOUND_ALL
					}
				}
			}
		};

		public static int DifficultyRatingForType(UnitType unitType)
		{
			const double HEALTH_MODIFER = 0.5f;
			const double DAMAGE_MODIFER = 1f;
			const double MOVE_MODIFER = 0.3f;

			var unitInfo = _staticUnitInfo[unitType];

			double result = 0;
			result += (unitInfo.damage * System.Math.Max(1, unitInfo.attackRange)) * HEALTH_MODIFER;
			result += unitInfo.movementRange * DAMAGE_MODIFER;
			result += unitInfo.maxHealth * MOVE_MODIFER;

			return (int)result;
		}

		public static List<UnitType> UnitsWithDifficulty(DifficultyRating rating)
		{
			var result = new List<UnitType>();
			
			foreach(UnitType unitType in System.Enum.GetValues(typeof(UnitType)).Cast<UnitType>())
			{
				if(_staticUnitInfo[unitType].difficultyRating == rating)
				{
					result.Add(unitType);
				}
			}

			return result;
		}

		private bool _moveTaken;

		private bool _attackTaken;

		public UnitType Type { get; set; }

		public Faction Faction { get; set; }

		public GameSubject GameSubject { get; set; }

		public bool MoveTaken
		{
			get => _moveTaken;
			set => _moveTaken = value;
		}

		public bool AttackTaken
		{
			get => _attackTaken;
			set => _attackTaken = value;
		}

		public bool TurnTaken
		{ 
			get
			{
				return _moveTaken && _attackTaken;
			}
		}

		public UnitSoundInfo SoundInfo
		{
			get => _staticUnitInfo[Type].SoundInfo;
		}

		public int MovementRange 
		{
			get => _staticUnitInfo[Type].movementRange;
		}

		public int AttackRange
		{
			get => _staticUnitInfo[Type].attackRange;
		}

		public Color BaseColor
		{
			get => _staticUnitInfo[Type].baseColor;
		}

		public double DamageAmount 
		{ 
			get => _staticUnitInfo[Type].damage;
		}

		public double MaxHealth
		{
			get => _staticUnitInfo[Type].maxHealth;
		}

		public int VisionRange
		{
			get => GameConstants.CHUNK_SIZE * 2;
		}

		public void TurnReset()
		{
			_moveTaken = false;
			_attackTaken = false;
		}

		protected override void InternalStart()
		{
			TurnReset();
		}

		public void OnNotify(IEnt ent, GameEvent evt)
		{
			switch(evt)
			{
				case GameEvent.TURN_END:
					// Cell -> Grid
					var cellCom = Owner.Parent.GetCom<CellInfoCom>();
					var gridCom = Owner.Parent.Parent.GetCom<GridCom>();
					foreach(var i in Utility.GetAdjacentSquares(cellCom.Cords.X, cellCom.Cords.Y, gridCom.Cells))
					{
						if(gridCom.Cells[i.X, i.Y].DamageAmount > 0)
						{
							Owner.Notify(cellCom.Owner, GameEvent.RECEIVE_DAMAGE);
						}
					}
					break;
			}
		}
	}
}