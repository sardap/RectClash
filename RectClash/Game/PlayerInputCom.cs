using RectClash.ECS;
using RectClash.ECS.Input;

namespace RectClash.Game
{
    public class PlayerInputCom : Com
    {
        private const float CAM_SPEED = 500.0f;

        public WorldCom World { get; set; }

        public override void Update()
        {
            var keyboard = Keyboard.Instance;

            if(keyboard.IsKeyPressed(Keys.Escape))
            {
                Engine.Instance.Window.Exit();
            }

            if(keyboard.IsKeyPressed(Keys.Space))
            {
                World.Grid.ChangeState(GridCom.State.TargetCellConf);
            }

            var deltaTime = Engine.Instance.Time.DeltaTimePercentOfSec;
            
            // Move Camrea
            if(keyboard.IsKeyPressed(Keys.D))
            {
                Engine.Instance.Window.Camera.X += CAM_SPEED * deltaTime;
            }
            if(keyboard.IsKeyPressed(Keys.A))
            {
                Engine.Instance.Window.Camera.X -= CAM_SPEED * deltaTime;
            }
            if(keyboard.IsKeyPressed(Keys.W))
            {
                Engine.Instance.Window.Camera.Y -= CAM_SPEED * deltaTime;
            }
            if(keyboard.IsKeyPressed(Keys.S))
            {
                Engine.Instance.Window.Camera.Y += CAM_SPEED * deltaTime;
            }
        }
    }
}