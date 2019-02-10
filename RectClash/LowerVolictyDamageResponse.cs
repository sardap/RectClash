using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using PaulECS;

namespace RectClash
{
	class LowerVolictyDamageResponse : IDamageResponse
	{
		private Vector2? _orginalSped = null;

		public float TotalSpeedMinusPercent { get; set; }

		public void Respond(IEntity entity, float maxHealth, float currentHealth)
		{
			if(_orginalSped == null)
			{
				_orginalSped = entity.GetCom<IMovementDataCom>().Volicty;
			}

			var healthPercent = currentHealth / maxHealth;

			var speedPercent = (healthPercent) * TotalSpeedMinusPercent;

			entity.GetCom<IMovementDataCom>().Volicty = ((Vector2)_orginalSped) * speedPercent;
		}
	}
}
