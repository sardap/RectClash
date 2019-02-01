using PaulECS;
using System;
using System.Collections.Generic;
using System.Text;

namespace RectClash
{
	class TeamCom: ICom
	{
		public string Team { get; set; }

		public long ID { get; set; }

		public IEntity Owner { get; set; }
	}
}
