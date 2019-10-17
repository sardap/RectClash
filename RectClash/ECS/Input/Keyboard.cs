using System.Collections.Generic;

namespace RectClash.ECS.Input
{
    public class Keyboard
    {
        private static volatile Keyboard _instance;

        public static Keyboard Instance { get { return _instance; } }

        private Keyboard(IKeyboardInput input)
        {
            _keyboard = input;
        }

        public static void Initialise(IKeyboardInput input)
        {
            if(_instance != null)
            {
                throw new System.InvalidOperationException("Already Created");
            }

            _instance = new Keyboard(input);
        }


        private IKeyboardInput _keyboard;

        public bool IsKeyPressed(Keys key)
        {
            return _keyboard.IsKeyPressed(key);
        }
    }
}