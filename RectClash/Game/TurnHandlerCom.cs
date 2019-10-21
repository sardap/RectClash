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

		public GameSubject Subjects { get; set; }
		
		public Subject<string, PerfEvents> DebugSubject { get; set; }

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
			EndTurn();
		}

		private void EndTurn()
		{
			Subjects.Notify(Owner, GameEvent.TURN_END);

			_index++;

			if(_index >= _turnOrder.Count)
			{
				_index = 0;
			}

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