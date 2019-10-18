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
                    break;

                default:
                    break;
                    throw new System.NotImplementedException();
            }
        }

        private void WindowKeyReleased(object sender, SFML.Window.KeyEventArgs e)
        {
            switch(e.Code)
            {
                case SFML.Window.Keyboard.Key.Escape:
                    Engine.Instance.Window.Exit();
                    break;

                case SFML.Window.Keyboard.Key.Space:
                    Subject.Notify(Owner, GameEvent.GRID_MOVE_CONF);
                    break;
            }
        }

        public override void Update()
        {
            var keyboard = Keyboard.Instance;

            var deltaTime = Engine.Instance.Time.DeltaTimePercentOfSec;
            
            // Move Camrea
            if(keyboard.IsKeyPressed(Keys.D))
            {
                Engine.Instance.Window.Camera.X -= CAM_SPEED * deltaTime;
            }
            if(keyboard.IsKeyPressed(Keys.A))
            {
                Engine.Instance.Window.Camera.X += CAM_SPEED * deltaTime;
            }
            if(keyboard.IsKeyPressed(Keys.W))
            {
                Engine.Instance.Window.Camera.Y += CAM_SPEED * deltaTime;
            }
            if(keyboard.IsKeyPressed(Keys.S))
            {
                Engine.Instance.Window.Camera.Y -= CAM_SPEED * deltaTime;
            }
        }
    }
}