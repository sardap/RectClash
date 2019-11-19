using System;
using RectClash.ECS;
using RectClash.Game.Unit;
using System.Linq;
using System.Collections.Generic;

namespace RectClash.Game.Perf
{
	public class DebugInputCom : Com
	{
		protected override void InternalStart()
		{
			Owner.Notify("Seed: \"" + Engine.Instance.Seed + "\"", PerfEvents.SEED_SET);
			
			Engine.Instance.Window.RenderWindow.KeyReleased += WindowKeyReleased;
			Owner.Notify(
				"Creating: " + Enum.GetName(EntFactory.Instance.UnitTypeToCreate.GetType(), EntFactory.Instance.UnitTypeToCreate),
				PerfEvents.UNIT_CREATE_SELECTION
			);

			Owner.Notify(
				"Faction: " + Enum.GetName(EntFactory.Instance.FactionToCreate.GetType(), EntFactory.Instance.FactionToCreate),
				PerfEvents.FACTION_SELECTION
			);
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
				Owner.Notify("Creating: " + Enum.GetName(unitToCreate.GetType(), unitToCreate), PerfEvents.UNIT_CREATE_SELECTION);
			}
			else if(e.Code == KeyBindsAccessor.Instance.CycleUnitType)
			{
				var factionTypes = Enum.GetValues(typeof(Faction)).Cast<Faction>().ToList();
				var i = factionTypes.IndexOf(EntFactory.Instance.FactionToCreate);

				i++;
				
				if(i >= factionTypes.Count)
				{
					i = 0;
				}

				var factionToCreate = factionTypes[i];

				EntFactory.Instance.FactionToCreate = factionToCreate;
				Owner.Notify("Faction: " + Enum.GetName(factionToCreate.GetType(), factionToCreate), PerfEvents.FACTION_SELECTION);
			}
		}
	}
}