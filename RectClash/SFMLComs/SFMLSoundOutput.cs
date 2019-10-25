using RectClash.ECS;
using SFML.Audio;
using System.Collections.Generic;
using RectClash.ECS.Sound;
using System.Linq;

namespace RectClash.Game.Sound
{
	public class SFMLSoundOutput : ISoundOutput
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

		public void PlaySound(string path)
		{
			GetSound(path).Play();
		}

		public void PlayRandomSound(IEnumerable<string> sounds)
		{
			if(sounds != null && sounds.Count() > 0 )
			{
				PlaySound(RectClash.Misc.Utility.RandomElement(sounds));
			}
		}
	}
}