using PaulECS;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace RectClash
{
	class LowerValueDamageResponse<ComT, VT>: IDamageResponse where VT : struct where ComT : ICom
	{
		private VT? _orginal = null;

		public PropertyInfo PropToGet { get; set; }

		public float TotalMinusPercent { get; set; }

		public void Respond(IEntity entity, float maxHealth, float currentHealth)
		{
			if (_orginal == null)
			{
				_orginal = (VT)PropToGet.GetValue(entity.GetCom<ComT>());
			}

			var healthPercent = currentHealth / maxHealth;

			var percent = (healthPercent) * TotalMinusPercent;

			dynamic dOrg = _orginal;

			PropToGet.SetValue(entity.GetCom<ComT>(), dOrg * percent);
		}
	}
}
