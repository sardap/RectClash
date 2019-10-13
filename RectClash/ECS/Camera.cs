using RectClash.Misc;

namespace RectClash.ECS
{
    public class Camera
    {
        public Vector2<double> Postion { get; set; }

        public double Zoom { get; set; }

        public Camera()
        {
            Postion = new Vector2<double>();
        }
    }
}