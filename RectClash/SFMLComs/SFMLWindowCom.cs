using RectClash.ECS;
using RectClash.misc;
using SFML.Graphics;

namespace RectClash.SFMLComs
{
    public class SFMLWindowCom : WindowCom
    {
        private RenderWindow window = null;

        public override bool IsOpen 
        { 
            get
            {
                return window.IsOpen;
            }
        }
        
        public override void OnStart()
        {
            window = new SFML.Graphics.RenderWindow(new SFML.Window.VideoMode((uint)Size.X, (uint)Size.Y), "SFML works!");            
        }

        public override void ProcessEvents()
        {
            window.DispatchEvents();
        }

        public override void Refresh()
        {
            window.Display();
        }

        public override void Exit()
        {
            window.Close();
        }
    }
}