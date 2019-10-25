using System.Collections.Generic;

namespace RectClash.Game.Unit
{
    public class UnitSoundInfo
    {
		public IEnumerable<string> DeathSound { get; set; }

		public IEnumerable<string> SelectSound { get; set; }

		public IEnumerable<string> MoveSound { get; set; }

		public IEnumerable<string> AttackSound { get; set; }
    }
}