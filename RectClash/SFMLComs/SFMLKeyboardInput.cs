using RectClash.ECS.Input;

namespace RectClash.SFMLComs
{
    public class SFMLKeyboardInput : IKeyboardInput
    {
        public bool IsKeyPressed(Keys key)
        {
            return SFML.Window.Keyboard.IsKeyPressed((SFML.Window.Keyboard.Key)key);
        }

    }
}