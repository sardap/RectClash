using RectClash.ECS;
using RectClash.Game.Combat;
using RectClash.Misc;

namespace RectClash.Game.Unit
{
	public class HealthCom : Com
	{		
		private double _currentHealth { get; set; }

		public double CurrentHealth 
		{ 
			get => _currentHealth;
		}
		
		public double MaxHealth { get; set; }

		public ProgressBarCom HealthBarCom { get; set; }

		public GameSubject Subject { get; set; }

		public void ReceiveDamage(IDamageInfoCom damageMessage)
		{
			var damageVariation = damageMessage.DamageAmount * GameConstants.UNIT_DAMAGE_VARIATION;
			var damageModifer = Utility.RandomDouble(-(damageVariation), damageVariation);

			_currentHealth -= damageMessage.DamageAmount + damageModifer;

			if(HealthBarCom != null)
				HealthBarCom.Percent = _currentHealth / MaxHealth;

			if(_currentHealth < 0)
			{
				Subject.Notify(Owner, GameEvent.UNIT_DIED);
			}
		}

		protected override void InternalStart()
		{
			_currentHealth = MaxHealth;
		}
	}
}