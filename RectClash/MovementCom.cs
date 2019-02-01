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

		public Vector2f Volicty { get; set; }

		public void Start()
		{
			_shapeCom = Owner.GetCom<RectangeShapeCom>();
			_shapeCom.Body.ApplyLinearImpulse(ToXNAVec(Volicty));
		}

		private Vector2 ToXNAVec(Vector2f a)
		{
			return new Vector2(a.X, a.Y);
		}
	}
}
