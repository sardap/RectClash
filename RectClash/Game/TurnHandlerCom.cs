using RectClash.ECS;
using RectClash.Game.Perf;
using RectClash.Game.Unit;
using RectClash.Misc;
using System.Collections.Generic;

namespace RectClash.Game
{
	public class TurnHandlerCom : Com, IGameObv
	{
		private int _index;

		private readonly List<Faction> _turnOrder = new List<Faction>();

		public IEnt GridEnt { get; set; }
		
		public DebugSubject DebugSubject { get; set; }

		public Faction Faction
		{
			get => _turnOrder[_index];
		}

		public IEnumerable<Faction> TurnOrder
		{
			get => _turnOrder;
		}

		public TurnHandlerCom()
		{
		}

		protected override void InternalStart()
		{
			foreach(var i in System.Enum.GetValues(typeof(Faction)))
			{
				_turnOrder.Add((Faction)i);
			}

			EndTurn();
		}

		private void EndTurn()
		{
			GridEnt.Notify(Owner, GameEvent.TURN_END);

			_index++;

			if(_index >= _turnOrder.Count)
			{
				_index = 0;
			}

			GridEnt.NotifyChildren(Owner, GameEvent.TURN_START);

			DebugSubject.Notify("Current Turn: " + Utility.GetEnumName(Faction), PerfEvents.TURN_CHANGED);
		}

		public void OnNotify(IEnt ent, GameEvent evt)
		{
			switch(evt)
			{
				case GameEvent.TRIGGER_TURN_END:
					EndTurn();
					break;
			}
		}
	}
}