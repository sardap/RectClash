using RectClash.ECS;
using RectClash.ECS.Input;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace RectClash.SFMLComs
{
    public class SFMLMouseInput : IMouseInput
    {
        private RenderWindow Window
        {
            get => ((SFMLWindow)Engine.Instance.Window).RenderWindow;
        }

        public Vector2i MousePostion
        {
            get => SFML.Window.Mouse.GetPosition(Window);
        }

        public float CamMouseX 
        { 
            get => MousePostion.X; 
        }

        public float CamMouseY 
        { 
            get => MousePostion.Y;
        }

        public float WorldMouseX
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public float WorldMouseY
        {
            get
            {
                throw new System.NotImplementedException();
            }
        }

        public Vector2f ToWorldMouse(Vector2f pos)
        {
            throw new System.NotImplementedException();
        }

        public bool IsButtonPressed(Mouse.Button button)
        {
            return Mouse.IsButtonPressed(button);
        }
    }
}