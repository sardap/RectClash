using RectClash.ECS;
using RectClash.ECS.Input;

namespace RectClash.Game
{
    public class PlayerInputCom : Com
    {
        private const float CAM_SPEED = 500.0f;

        public GameSubject Subject { get; set; }

        protected override void InternalStart()
        {
            Engine.Instance.Window.RenderWindow.KeyReleased += WindowKeyReleased;
            Engine.Instance.Window.RenderWindow.MouseWheelScrolled += MouseWheelScrolled;
        }

        private void MouseWheelScrolled(object sender, SFML.Window.MouseWheelScrollEventArgs e)
        {
            switch(e.Wheel)
            {
                case SFML.Window.Mouse.Wheel.VerticalWheel:
                    Engine.Instance.Window.Camera.ZoomX += e.Delta * 0.2f;
                    Engine.Instance.Window.Camera.ZoomY += e.Delta * 0.2f;

					// if(e.Delta > 0)
					// {
					// 	Engine.Instance.Window.Camera.X -= e.Delta * 0.2f;
					// 	Engine.Instance.Window.Camera.Y -= e.Delta * 0.2f;
					// }
					// else
					// {
					// 	Engine.Instance.Window.Camera.X += e.Delta * 0.2f;
					// 	Engine.Instance.Window.Camera.Y += e.Delta * 0.2f;
					// }

                    break;

                default:
                    break;
                    throw new System.NotImplementedException();
            }
        }

        private void WindowKeyReleased(object sender, SFML.Window.KeyEventArgs e)
        {
			if(e.Code == KeyBindsAccessor.Instance.QuitProgram)
			{
				Engine.Instance.Window.Exit();
			}
			else if(e.Code == KeyBindsAccessor.Instance.EndTurn)
			{
				Subject.Notify(Owner, GameEvent.TRIGGER_TURN_END);
			}
			else if(e.Code == KeyBindsAccessor.Instance.ConfMove)
			{
				Subject.Notify(Owner, GameEvent.GRID_MOVE_CONF);
			}
        }

        public override void Update()
        {
            var deltaTime = Engine.Instance.Time.DeltaTimePercentOfSec;
            
            // Move Camrea
            if(SFML.Window.Keyboard.IsKeyPressed(KeyBindsAccessor.Instance.MoveCameraRight))
            {
                Engine.Instance.Window.Camera.X -= CAM_SPEED * deltaTime;
            }
            if(SFML.Window.Keyboard.IsKeyPressed(KeyBindsAccessor.Instance.MoveCameraLeft))
            {
                Engine.Instance.Window.Camera.X += CAM_SPEED * deltaTime;
            }
            if(SFML.Window.Keyboard.IsKeyPressed(KeyBindsAccessor.Instance.MoveCameraUp))
            {
                Engine.Instance.Window.Camera.Y += CAM_SPEED * deltaTime;
            }
            if(SFML.Window.Keyboard.IsKeyPressed(KeyBindsAccessor.Instance.MoveCameraDown))
            {
                Engine.Instance.Window.Camera.Y -= CAM_SPEED * deltaTime;
            }
        }
    }
}