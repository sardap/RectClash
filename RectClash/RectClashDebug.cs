using PaulECS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace RectClash
{
	public static class RectClashDebug
	{
		public static string GenIDPart(IEntity entity)
		{
			return "ID: " + entity.ID + " ";
		}
	}
}
