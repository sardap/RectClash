using System.Collections.Generic;
using Priority_Queue;
using RectClash.ECS;
using RectClash.ECS.Graphics;
using RectClash.Misc;
using SFML.Graphics;
using SFML.System;

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

        public RenderWindow Window
        {
            get => _window;
        }

        public override void OnStart()
        {
            _window = new SFML.Graphics.RenderWindow(new SFML.Window.VideoMode((uint)Size.X, (uint)Size.Y), "SFML works!");
            _workingDIR = System.Environment.GetEnvironmentVariable("RES_DIR");
            Camera = new Camera()
            {
                Postion = new Vector2f(0, 0)
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

        private RenderStates _states = RenderStates.Default;

        public override void DrawDrawQueue(SimplePriorityQueue<IDrawableCom> drawQueue)
        {
            while(drawQueue.Count > 0)
            {
                var top = drawQueue.Dequeue();
                SFML.Graphics.Drawable toDraw;

                if(top is DrawCircleCom)
                {
                    var cirlce = (DrawCircleCom)top;
                    _sfmlCircle.Position = top.Owner.PostionCom.Postion;
                    _sfmlCircle.Radius = (float)cirlce.Radius;
                    _sfmlCircle.FillColor = cirlce.Color;

                    toDraw = _sfmlCircle;
                }
                else if(top is RenderTextCom)
                {
                    var text = (RenderTextCom)top;
                    _sfmlText.Position = top.Owner.PostionCom.Postion;
                    _sfmlText.Font = LoadFont(text.Font.FileLocation);
                    _sfmlText.FillColor = ConvertToSFMLColor(text.Color);
                    _sfmlText.DisplayedString = text.Text;
                    
                    toDraw = _sfmlText;
                }
                else if(top is DrawRectCom)
                {
                    var rect = (DrawRectCom)top;
                    _sfmlRect.Position = rect.PostionCom.Postion;
                    _sfmlRect.Size = rect.PostionCom.Size;

                    _sfmlRect.FillColor = rect.FillColor;
                    _sfmlRect.OutlineColor = rect.OutlineColor;
                    _sfmlRect.OutlineThickness = (float)rect.LineThickness;

                    toDraw = _sfmlRect;
                }
                else
                {
                    continue;
                }

                _states.Transform = top.Owner.PostionCom.Transform;

                if(top.Floating)
                {
                    _window.SetView(_window.DefaultView);
                }
                else
                {
                    _window.SetView(Camera.View);
                }

                _window.Draw(toDraw, _states);
            }
        }

        private Color ConvertToSFMLColor(Color col)
        {
            return new Color(col.R, col.G, col.B);
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