using System.Collections.Generic;

namespace RectClash.Game
{
    public static class GameConstants
    {
        public const float DAMAGE_VARIATION = 0.3f;

		public const int MELEE_ATTACK_RANGE = 0;

		public const int CHUNK_SIZE = 10;

		public const string KEY_BINDING_FILE = "Keybinds.json";

		public static readonly IEnumerable<string> SOUND_FOOT_SOLIDER_ATTACK = new List<string>()
		{
			"Sounds/FootSolider/attack_01.wav",
			"Sounds/FootSolider/attack_02.wav"
		};

		public static readonly IEnumerable<string> SOUND_FOOT_SOLIDER_MOVE = new List<string>()
		{
			"Sounds/FootSolider/move_01.wav",
			"Sounds/FootSolider/move_02.wav",
			"Sounds/FootSolider/move_03.wav",
			"Sounds/FootSolider/move_04.wav"
		};

		public static readonly IEnumerable<string> SOUND_FOOT_SOLIDER_SELECTED = new List<string>()
		{
			"Sounds/FootSolider/select_01.wav",
			"Sounds/FootSolider/select_02.wav"
		};

		public static readonly IEnumerable<string> SOUND_FOOT_SOLIDER_DIED = new List<string>()
		{
			"Sounds/FootSolider/death_sound_01.wav"
		};

		public static readonly IEnumerable<string> SOUND_HEAVY_SOLIDER_ATTACK = new List<string>()
		{
			"Sounds/HeavySolider/attack_01.wav"
		};

		public static readonly IEnumerable<string> SOUND_HEAVY_SOLIDER_MOVE = new List<string>()
		{
			"Sounds/HeavySolider/move_01.wav"
		};

		public static readonly IEnumerable<string> SOUND_HEAVY_SOLIDER_SELECTED = new List<string>()
		{
			"Sounds/HeavySolider/select_01.wav",
			"Sounds/HeavySolider/select_02.wav"
		};

		public static readonly IEnumerable<string> SOUND_HEAVY_SOLIDER_DIED = new List<string>()
		{
			"Sounds/HeavySolider/death_01.wav",
			"Sounds/HeavySolider/death_02.wav",
			"Sounds/HeavySolider/death_03.wav"
		};

		public static readonly IEnumerable<string> SOUND_LIGHT_ARCHER_ATTACK = new List<string>()
		{
			"Sounds/HeavySolider/attack_01.wav"
		};

		public static readonly IEnumerable<string> SOUND_LIGHT_ARCHER_MOVE = new List<string>()
		{
			"Sounds/HeavySolider/move_01.wav"
		};

		public static readonly IEnumerable<string> SOUND_LIGHT_ARCHER_SELECTED = new List<string>()
		{
			"Sounds/HeavySolider/select_01.wav",
			"Sounds/HeavySolider/select_02.wav"
		};

		public static readonly IEnumerable<string> SOUND_LIGHT_ARCHER_DIED = new List<string>()
		{
			"Sounds/HeavySolider/death_01.wav",
			"Sounds/HeavySolider/death_02.wav",
			"Sounds/HeavySolider/death_03.wav"
		};
    }
}