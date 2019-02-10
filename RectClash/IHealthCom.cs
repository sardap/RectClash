using PaulECS;
using System;
using System.Collections.Generic;
using System.Text;

namespace RectClash
{
	interface IHealthCom: ICom
	{
		IList<IDamageResponse> DamageResponses { get; set; }

		float MaxHealth { get; set; }

		float CurrentHealth { get; }

		void DealDamage(float amount);
	}
}
