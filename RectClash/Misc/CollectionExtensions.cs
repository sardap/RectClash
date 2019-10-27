using System;
using System.Collections.Generic;
using System.Linq;
using RectClash.ECS;
using SFML.Graphics;

namespace RectClash.Misc
{
    public static class CollectionExtensions
    {
		public static ICollection<T> RemoveAll<T>(this ICollection<T> list, ICollection<T> toRemove)
		{
			foreach(var i in toRemove)
			{
				list.Remove(i);
			}
			
			return list;
		}
    }
}