using System.Collections.Generic;

namespace RectClash.Game
{
    public static class GameConstants
    {
        public const float DAMAGE_VARIATION = 0.3f;

		public const string KEY_BINDING_FILE = "Keybinds.json";

		public const string SOUND_FOOT_SOLIDER_DIED = "death_sound.flac";

		public static readonly IEnumerable<string> SOUND_FOOT_SOLIDER_ATTACK = new List<string>()
		{
			"Sounds/attack_01.wav",
			"Sounds/attack_02.wav"
		};

		public static readonly IEnumerable<string> SOUND_FOOT_SOLIDER_MOVE = new List<string>()
		{
			"Sounds/move_01.wav",
			"Sounds/move_02.wav",
			"Sounds/move_03.wav",
			"Sounds/move_04.wav"
		};

		public static readonly IEnumerable<string> SOUND_FOOT_SOLIDER_SELECTED = new List<string>()
		{
			"Sounds/select_01.wav",
			"Sounds/select_02.wav"
		};
    }
}