using Microsoft.Xna.Framework;
using PaulECS;
using PaulECS.SFMLComs;
using SFML.System;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RectClash
{
	class MovementCom : ICom, IHaveStart, IHaveUpdate
	{
		private RectangeShapeCom _shapeCom;
		private Vector2 _lastPostion;
		private bool _moving;

		public long ID { get; set; }

		public IEntity Owner { get; set; }

		public bool Moving
		{
			get
			{
				return _moving;
			}
		}

		public void Start()
		{
			_shapeCom = Owner.GetCom<RectangeShapeCom>();
		}

		public void Stop()
		{
			_shapeCom.Body.LinearVelocity = new Vector2();
		}

		public void ApplyVolicty(Vector2 volicty)
		{
			_shapeCom.Body.LinearVelocity = new Vector2(); 
			_shapeCom.Body.ApplyLinearImpulse(volicty);

			Debug.WriteLine(RectClashDebug.GenIDPart(Owner) + " " + volicty, DebugCatagroys.MOVEMENT);
		}

		public void Update(double deltaTime)
		{
			_moving = _lastPostion != Owner.GetCom<RectangeShapeCom>().Body.Position;

			_lastPostion = Owner.GetCom<RectangeShapeCom>().Body.Position;
		}
	}
}
