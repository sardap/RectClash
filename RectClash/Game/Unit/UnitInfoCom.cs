using RectClash.ECS;
using System.Collections.Generic;
using RectClash.ECS.Graphics;
using SFML.Graphics;

namespace RectClash.Game.Unit
{
	public class UnitInfoCom : Com
	{
		private class StaticUnitInfo
		{
			public double maxHealth;
			public double damage;
			public Color baseColor;
			public int range;

			public UnitSoundInfo SoundInfo;

		}

		private static Dictionary<UnitType, StaticUnitInfo> _staticUnitInfo = new Dictionary<UnitType, StaticUnitInfo>()
		{
			{
				UnitType.Regular,
				new StaticUnitInfo()
				{
					damage = 10.0d,
					baseColor = Color.White,
					range = 5,
					maxHealth = 30d,
					SoundInfo = new UnitSoundInfo()
					{
						DeathSound = GameConstants.SOUND_FOOT_SOLIDER_DIED,
						SelectSound = GameConstants.SOUND_FOOT_SOLIDER_SELECTED,
						MoveSound = GameConstants.SOUND_FOOT_SOLIDER_MOVE,
						AttackSound = GameConstants.SOUND_FOOT_SOLIDER_ATTACK
					}
				}
			},
			{
				UnitType.Heavy,
				new StaticUnitInfo()
				{
					damage = 20.0d,
					baseColor = Color.Magenta,
					range = 3,
					maxHealth = 60d,
					SoundInfo = new UnitSoundInfo()
					{
						DeathSound = GameConstants.SOUND_HEAVY_SOLIDER_DIED,
						SelectSound = GameConstants.SOUND_HEAVY_SOLIDER_SELECTED,
						MoveSound = GameConstants.SOUND_HEAVY_SOLIDER_MOVE,
						AttackSound = GameConstants.SOUND_HEAVY_SOLIDER_ATTACK
					}
				}
			}
		};

		private bool _turnTaken;

		
		public UnitType Type { get; set; }

		public Faction Faction { get; set; }

		public GameSubject GameSubject { get; set; }

		public bool TurnTaken 
		{ 
			get => _turnTaken; 
			set
			{
				_turnTaken = value;
			} 
		}

		public UnitSoundInfo SoundInfo
		{
			get => _staticUnitInfo[Type].SoundInfo;
		}

		public int Range 
		{
			get => _staticUnitInfo[Type].range;
		}

		public Color BaseColor
		{
			get => _staticUnitInfo[Type].baseColor;
		}

		public double Damage 
		{ 
			get => _staticUnitInfo[Type].damage;
		}

		public double MaxHealth
		{
			get => _staticUnitInfo[Type].maxHealth;
		}

		protected override void InternalStart()
		{
			TurnTaken = false;
		}
    }
}