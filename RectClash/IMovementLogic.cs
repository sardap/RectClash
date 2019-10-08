using System;
using System.Collections.Generic;
using System.Text;
using PaulECS;

namespace RectClash
{
	interface IMovementLogic
	{
		void Step(IEntity entity);
	}
}
