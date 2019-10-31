using RectClash.ECS;
using System.Collections.Generic;
using RectClash.ECS.Graphics;
using SFML.Graphics;
using RectClash.Game.Combat;

namespace RectClash.Game.Unit
{
	public class UnitInfoCom : Com, IDamageInfoCom
	{
		private class CommonUnitInfo
		{
			public double maxHealth;
			public double damage;
			public Color baseColor;
			public int movementRange;
			public int attackRange;
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

		public int attackRange
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

		public void TurnReset()
		{
			_moveTaken = false;
			_attackTaken = false;
		}

		protected override void InternalStart()
		{
			TurnReset();
		}
    }
}