using RectClash.ECS;
using RectClash.ECS.Graphics;

namespace RectClash.Game
{
    public class CircleMove : Com
    {
        public double Speed { get; set; }

        public DrawCircleCom DrawCircle { get; set; }

        public override void Update()
        {
            var realSpeed = (Speed * Engine.Instance.Time.DeltaTime);
            DrawCircle.PostionCom.X += realSpeed;

            if(DrawCircle.PostionCom.X + (DrawCircle.Radius / 2) > Engine.Instance.Window.Size.X)
            {
                DrawCircle.PostionCom.X = 0 - DrawCircle.Radius * 2;
            }
        }
    }
}