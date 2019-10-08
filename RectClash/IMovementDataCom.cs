using Microsoft.Xna.Framework;
using PaulECS;
using System;
using System.Collections.Generic;
using System.Text;

namespace RectClash
{
	interface IMovementDataCom: ICom
	{
		float Speed { get; set; }

		Vector2 Postion { get; }
	}
}
