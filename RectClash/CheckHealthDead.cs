using System;
using System.Collections.Generic;
using System.Text;
using PaulECS;

namespace RectClash
{
	class CheckHealthDead : DeathCom.ICheckDead
	{
		public bool CheckDead(IEntity entity)
		{
			return entity.GetCom<IHealthCom>().CurrentHealth < 0;
		}
	}
}
