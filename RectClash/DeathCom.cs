using PaulECS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RectClash
{
	class DeathCom : ICom, IHaveUpdate
	{
		public interface ICheckDead
		{
			bool CheckDead(IEntity entity);
		}

		public long ID { get; set; }

		public IEntity Owner { get; set; }

		public List<ICheckDead> Checks { get; set; }

		public void Update(double deltaTime)
		{
			if (Checks.TrueForAll(i => i.CheckDead(Owner)))
			{
				Debug.WriteLine("KILLING ID: {0}", Owner.ID);
				Owner.KillMe();
			}
		}
	}
}
