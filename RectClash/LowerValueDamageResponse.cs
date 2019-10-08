using PaulECS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

			if (currentHealth > maxHealth)
				return;

			var healthPercent = currentHealth / maxHealth;

			var percent = ((healthPercent) *  (1 - TotalMinusPercent));

			dynamic dOrg = _orginal;

			PropToGet.SetValue(entity.GetCom<ComT>(), dOrg * percent);

			Debug.WriteLine(RectClashDebug.GenIDPart(entity) + " " + percent + " " + PropToGet.Name, DebugCatagroys.LOWER_VALUE);
		}
	}
}
