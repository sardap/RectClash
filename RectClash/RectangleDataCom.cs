using Microsoft.Xna.Framework;
using PaulECS;
using PaulECS.SFMLComs;
using System;
using System.Collections.Generic;
using System.Text;

namespace RectClash
{
	class RectangleDataCom : IHealthCom, IHaveStart, IMovementDataCom, ITeamData, IAgilityDataCom
	{
		public float CurrentHealth { get; set; }

		public float MaxHealth { get; set; }

		public long ID { get; set; }

		public IEntity Owner { get; set; }

		public IList<IDamageResponse> DamageResponses { get; set; }
		
		public float Speed { get; set; }

		public float Agility { get; set; }

		public TeamInfo TeamInfo { get; set; }

		public Vector2 Postion
		{
			get
			{
				return Owner.GetCom<RectangeShapeCom>().Body.Position;
			}
		}

		public void DealDamage(float amount)
		{
			CurrentHealth -= amount;

			foreach(IDamageResponse damageResponse in DamageResponses)
			{
				damageResponse.Respond(Owner, MaxHealth, CurrentHealth);
			}
		}

		public void Start()
		{
			if(DamageResponses == null)
				DamageResponses = new List<IDamageResponse>();
		}
	}
}
