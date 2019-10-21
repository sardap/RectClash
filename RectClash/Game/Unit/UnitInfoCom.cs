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
					maxHealth = 30d
				}
			},
			{
				UnitType.Heavy,
				new StaticUnitInfo()
				{
					damage = 20.0d,
					baseColor = Color.Magenta,
					range = 3,
					maxHealth = 60d
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