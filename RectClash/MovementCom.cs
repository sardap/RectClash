using Microsoft.Xna.Framework;
using PaulECS;
using PaulECS.SFMLComs;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Text;

namespace RectClash
{
	class MovementCom : ICom, IHaveStart
	{
		private RectangeShapeCom _shapeCom;

		public long ID { get; set; }

		public IEntity Owner { get; set; }

		public bool Moving
		{
			get
			{
				return _shapeCom.Body.LinearVelocity != new Vector2(0, 0);
			}
		}

		public void Start()
		{
			_shapeCom = Owner.GetCom<RectangeShapeCom>();
		}

		public void ApplyVolicty()
		{
			_shapeCom.Body.ApplyLinearImpulse(Owner.GetCom<IMovementDataCom>().Volicty);

			//_shapeCom.Body.LinearVelocity = Owner.GetCom<IMovementDataCom>().Volicty;
		}
	}
}
