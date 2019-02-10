using PaulECS;
using System;
using System.Collections.Generic;
using System.Text;

namespace RectClash
{
	interface ITeamData: ICom
	{
		string Team { get; set; }
	}
}
