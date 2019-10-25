using System.Collections.Generic;

namespace RectClash.ECS.Sound
{
    public interface ISoundOutput
    {
        void PlaySound(string path);

		void PlayRandomSound(IEnumerable<string> paths);
    }
}