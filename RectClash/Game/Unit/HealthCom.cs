using RectClash.ECS;

namespace RectClash.Game.Unit
{
	public class HealthCom : Com
	{
		private double _currentHealth { get; set; }

		public double CurrentHealth 
		{ 
			get => _currentHealth;
			set
			{
				_currentHealth = value;
				if(HealthBarCom != null)
					HealthBarCom.Percent = _currentHealth / MaxHealth;

				if(_currentHealth < 0)
				{
					Owner.Destory();
				}
			}
		}
		
		public double MaxHealth { get; set; }

		public ProgressBarCom HealthBarCom { get; set; }

		protected override void InternalStart()
		{
			CurrentHealth = MaxHealth;
		}
	}
}