using RectClash.ECS;
using RectClash.ECS.Input;

namespace RectClash.Game
{
    public class PlayerInputCom : Com
    {
        private const double CAM_SPEED = 500.0;

        public override void Update()
        {
            var keyboard = Keyboard.Instance;

            if(keyboard.IsKeyPressed(Keys.Escape))
            {
                Engine.Instance.Window.Exit();
            }

            var deltaTime = Engine.Instance.Time.DeltaTimePercentOfSec;
            
            // Move Camrea
            if(keyboard.IsKeyPressed(Keys.Right))
            {
                Engine.Instance.Window.Camera.Postion.X -= CAM_SPEED * deltaTime;
            }
            if(keyboard.IsKeyPressed(Keys.Left))
            {
                Engine.Instance.Window.Camera.Postion.X += CAM_SPEED * deltaTime;
            }
            if(keyboard.IsKeyPressed(Keys.Up))
            {
                Engine.Instance.Window.Camera.Postion.Y += CAM_SPEED * deltaTime;
            }
            if(keyboard.IsKeyPressed(Keys.Down))
            {
                Engine.Instance.Window.Camera.Postion.Y -= CAM_SPEED * deltaTime;
            }
        }
    }
}