using RectClash.ECS;
using RectClash.ECS.Input;

namespace RectClash
{
	class SimpleWindow
    {
        public void Run()
        {
            var mode = new SFML.Window.VideoMode(800, 600);
            var window = new SFML.Graphics.RenderWindow(mode, "SFML works!");
            window.KeyPressed += Window_KeyPressed;

            var circle = new SFML.Graphics.CircleShape(100f)
            {
                FillColor = SFML.Graphics.Color.Blue
            };

            // Start the game loop
            while (window.IsOpen)
            {
                // Process events
                window.DispatchEvents();
                window.Draw(circle);

                // Finally, display the rendered frame on screen
                window.Display();
            }
        }

        /// <summary>
        /// Function called when a key is pressed
        /// </summary>
        private void Window_KeyPressed(object sender, SFML.Window.KeyEventArgs e)
        {
            var window = (SFML.Window.Window)sender;
            if (e.Code == SFML.Window.Keyboard.Key.Escape)
            {
                window.Close();
            }
        }
    }
	
	class Program
	{
		static void Main(string[] args)
		{
			Keyboard.Initialise(new SFMLComs.SFMLKeyboardInput());	

			IWindowCom windowCom = new SFMLComs.SFMLWindowCom()
			{
				Size = new misc.Vector2<int>(800, 600)
			};

			windowCom.OnStart();

			var done = false;
			while(!done)
			{
				windowCom.Update();

				if(Keyboard.Instance.IsKeyPressed(Keys.Escape))
				{
					windowCom.Exit();
					done = true;
				}
			}

		}
	}
}