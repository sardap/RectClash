using Microsoft.Xna.Framework;
using PaulECS;
using System;
using System.Collections.Generic;
using System.Text;

namespace RectClash
{
	interface IMovementDataCom: ICom
	{
		Vector2 Volicty { get; set; }
	}
}
