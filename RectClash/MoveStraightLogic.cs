using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using PaulECS;

namespace RectClash
{
	class MoveStraightLogic : IMovementLogic
	{
		public Vector2 Volcity { get; set; }

		public void Step(IEntity entity)
		{
			if (!entity.GetCom<MovementCom>().Moving)
			{
				Debug.WriteLineIf(Volcity.X == 0f && Volcity.Y == 0f, "ID:" + entity.ID + " Advanceing ", DebugCatagroys.AI_STATE_MACHINE);
				entity.GetCom<MovementCom>().ApplyVolicty(Volcity);
			}
		}
	}
}
