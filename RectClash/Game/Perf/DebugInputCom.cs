using System;
using RectClash.ECS;
using RectClash.Game.Unit;

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
				UnitType unitToCreate;
				if(EntFactory.Instance.UnitTypeToCreate == UnitType.Regular)
				{
					unitToCreate = UnitType.Heavy;
				}
				else
				{
					unitToCreate = UnitType.Regular;
				}
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