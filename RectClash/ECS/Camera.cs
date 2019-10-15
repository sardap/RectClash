using RectClash.Misc;
using RectClash.SFMLComs;
using SFML.Graphics;
using SFML.System;

namespace RectClash.ECS
{
    public class Camera
    {
        private View _view;

        public View View
        {
            get => _view;
        }

        public Vector2f Postion 
        { 
            get => _view.Center;
            set
            {
                _view.Center = value;
                Dirty = true;
            }
        }

        public float X
        {
            get => Postion.X;
            set => Postion = new Vector2f(value, Y);
        }

        public float Y
        {
            get => Postion.Y;
            set => Postion = new Vector2f(X, value);
        }

        public double Zoom 
        { 
            get; 
            set; 
        }

        public bool Dirty
        {
            get;
            set;
        }

        public Camera()
        {
            _view = new View();
            _view.Size = Engine.Instance.Window.Size;
        }
    }
}