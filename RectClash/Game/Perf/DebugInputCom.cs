using System;
using RectClash.ECS;
using RectClash.Game.Unit;
using System.Linq;
using System.Collections.Generic;

namespace RectClash.Game.Perf
{
	public class DebugInputCom : Com
	{
		public Subject<string, PerfEvents> Subject { get; set; }

		protected override void InternalStart()
		{
			Engine.Instance.Window.RenderWindow.KeyReleased += WindowKeyReleased;
			Subject.Notify("Creating: " + Enum.GetName(EntFactory.Instance.UnitTypeToCreate.GetType(), EntFactory.Instance.UnitTypeToCreate), PerfEvents.UNIT_CREATE_SELECTION);
			Subject.Notify("Faction: " + Enum.GetName(EntFactory.Instance.FactionToCreate.GetType(), EntFactory.Instance.FactionToCreate), PerfEvents.FACTION_SELECTION);
		}

		private void WindowKeyReleased(object sender, SFML.Window.KeyEventArgs e)
		{
			if(e.Code == KeyBindsAccessor.Instance.ToggleTeamSelection)
			{
				var unitTypes = Enum.GetValues(typeof(UnitType)).Cast<UnitType>().ToList();
				var i = unitTypes.IndexOf(EntFactory.Instance.UnitTypeToCreate);

				i++;
				
				if(i >= unitTypes.Count)
				{
					i = 0;
				}

				var unitToCreate = unitTypes[i];
				EntFactory.Instance.UnitTypeToCreate = unitToCreate;
				Subject.Notify("Creating: " + Enum.GetName(unitToCreate.GetType(), unitToCreate), PerfEvents.UNIT_CREATE_SELECTION);
			}
			else if(e.Code == KeyBindsAccessor.Instance.CycleUnitType)
			{
				Faction faction;
				if(EntFactory.Instance.FactionToCreate == Faction.Red)
				{
					faction = Faction.Blue;
				}
				else
				{
					faction = Faction.Red;
				}
				EntFactory.Instance.FactionToCreate = faction;
				Subject.Notify("Faction: " + Enum.GetName(faction.GetType(), faction), PerfEvents.FACTION_SELECTION);
			}
		}
	}
}