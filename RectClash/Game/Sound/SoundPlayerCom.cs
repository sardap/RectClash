using RectClash.ECS;
using SFML.Audio;
using System.Collections.Generic;

namespace RectClash.Game.Sound
{
	public class SoundPlayerCom : Com, IGameObv
	{
		private readonly Dictionary<string, SFML.Audio.Sound> _loadedSounds = new Dictionary<string, SFML.Audio.Sound>();

		private SFML.Audio.Sound GetSound(string path)
		{
			if(!_loadedSounds.ContainsKey(path))
			{
				var fullPath = System.IO.Path.Combine(System.Environment.GetEnvironmentVariable("RES_DIR"), path);

				var newSound = new SoundBuffer(fullPath);
				_loadedSounds.Add(path, new SFML.Audio.Sound(newSound));
			}

			return _loadedSounds[path];
		}

		private void PlaySound(string path)
		{
			GetSound(path).Play();
		}

		public void OnNotify(IEnt ent, GameEvent evt)
		{
			switch(evt)
			{
				case GameEvent.FOOT_SOLIDER_DIED:
					PlaySound(GameConstants.SOUND_FOOT_SOLIDER_DIED);
					break;

				case GameEvent.GRID_CELL_SELECTED:
					break;
			}
		}
	}
}