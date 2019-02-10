using PaulECS;
using System;
using System.Collections.Generic;
using System.Text;

namespace RectClash
{
	interface IDamageResponse
	{
		void Respond(IEntity entity, float maxHealth, float currentHealth);
	}
}
