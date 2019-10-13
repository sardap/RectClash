using System.Collections.Generic;
using RectClash.ECS;
using RectClash.ECS.Graphics;
using RectClash.Misc;
using SFML.Graphics;

namespace RectClash.SFMLComs
{
    public class SFMLWindow : Window
    {
        private Dictionary<string, SFML.Graphics.Font> _loadedFonts = new Dictionary<string, SFML.Graphics.Font>();

        private RenderWindow _window = null;

        private string _workingDIR;

        public override bool IsOpen 
        { 
            get
            {
                return _window.IsOpen;
            }
        }

        public override void OnStart()
        {
            _window = new SFML.Graphics.RenderWindow(new SFML.Window.VideoMode((uint)Size.X, (uint)Size.Y), "SFML works!");
            _workingDIR = System.Environment.GetEnvironmentVariable("RES_DIR");
            Camera = new Camera()
            {
                Postion = new Vector2<double>(Size.X / 2, Size.Y / 2)
            };
        }

        public override void ProcessEvents()
        {
            _window.DispatchEvents();
        }

        public override void Refresh()
        {
            _window.Display();
        }

        public override void Exit()
        {
            _window.Close();
        }

        public override void Clear()
        {
            _window.Clear();
        }

        private CircleShape _sfmlCircle = new CircleShape();
        private SFML.Graphics.Text _sfmlText = new SFML.Graphics.Text();

        private RectangleShape _sfmlRect = new RectangleShape();

        private SFML.System.Vector2f _sfmlPostion = new SFML.System.Vector2f();

        private RenderStates _states = RenderStates.Default;

        public override void DrawDrawQueue(Queue<IDrawableCom> drawQueue)
        {
            while(drawQueue.Count > 0)
            {
                var top = drawQueue.Dequeue();
                SFML.Graphics.Drawable toDraw;

                if(top is DrawCircleCom)
                {
                    var cirlce = (DrawCircleCom)top;
                    _sfmlCircle.Radius = (float)cirlce.Radius;
                    _sfmlCircle.Position = ConvertToSFMLVector(cirlce.Owner.PostionCom);
                    _sfmlCircle.FillColor = ConvertToSFMLColour(cirlce.Colour);

                    toDraw = _sfmlCircle;
                }
                else if(top is RenderTextCom)
                {
                    var text = (RenderTextCom)top;
                    _sfmlText.Font = LoadFont(text.Font.FileLocation);
                    _sfmlText.Position = ConvertToSFMLVector(text.Owner.PostionCom);
                    _sfmlText.FillColor = ConvertToSFMLColour(text.Colour);
                    _sfmlText.DisplayedString = text.Text;
                    
                    toDraw = _sfmlText;
                }
                else if(top is DrawRectCom)
                {
                    var rect = (DrawRectCom)top;
                    _sfmlRect.Position = ConvertToSFMLVector(rect.Owner.PostionCom);
                    
                    _sfmlPostion.X = (float)rect.Width;
                    _sfmlPostion.Y = (float)rect.Height;
                    _sfmlRect.Size = _sfmlPostion;

                    _sfmlRect.FillColor = new SFML.Graphics.Color(0, 0, 0, 0);
                    _sfmlRect.OutlineColor = ConvertToSFMLColour(rect.OutlineColour);
                    _sfmlRect.OutlineThickness = (float)rect.LineThickness;

                    toDraw = _sfmlRect;
                }
                else
                {
                    continue;
                }

                if(!top.Floating)
                {
                    Transform t = Transform.Identity;
                    //t.TransformPoint((float)Camera.Postion.X, (float)Camera.Postion.Y);
                    t.TransformPoint((float)Camera.Postion.X, (float)Camera.Postion.Y);
                    t.Translate((float)Camera.Postion.X, (float)Camera.Postion.Y);
                    _states.Transform = t;    
                }
                else
                {
                    _states.Transform = Transform.Identity;
                }

                _window.Draw(toDraw, _states);
            }
        }

        private Color ConvertToSFMLColour(Colour col)
        {
            return new Color(col.R, col.G, col.B);
        }

        private SFML.System.Vector2f ConvertToSFMLVector(PostionCom com)
        {
            var comsSFMLVec = new SFML.System.Vector2f()
            {
                X = (float)com.X,
                Y = (float)com.Y
            };

            if(com.Owner.Parent != null)
            {
                var offset = ConvertToSFMLVector(com.Owner.Parent.PostionCom);

                comsSFMLVec += offset;
            }

            return comsSFMLVec;
        }

        private SFML.Graphics.Font LoadFont(string fileLocation)
        {
            if(!_loadedFonts.ContainsKey(fileLocation))
            {
                var fullPath = System.IO.Path.Combine(_workingDIR, fileLocation);
                _loadedFonts.Add(fileLocation, new SFML.Graphics.Font(fullPath));
            }

            return _loadedFonts[fileLocation];
        }
    }
}