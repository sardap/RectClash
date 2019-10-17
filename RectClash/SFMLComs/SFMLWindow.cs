using System;
using System.Collections.Generic;
using Priority_Queue;
using RectClash.ECS;
using RectClash.ECS.Graphics;
using RectClash.Misc;
using SFML.Graphics;
using SFML.System;
using SFML.Window;

namespace RectClash.SFMLComs
{
    public class SFMLWindow : ECS.Window
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

        public override RenderWindow RenderWindow
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

        private SFML.Graphics.Drawable _toDraw;

        public override void DrawDrawQueue(SimplePriorityQueue<IDrawableCom> drawQueue)
        {
            while(drawQueue.Count > 0)
            {
                var top = drawQueue.Dequeue();

                if(top is DrawCircleCom)
                {
                    var cirlce = (DrawCircleCom)top;
                    _sfmlCircle.Position = top.Owner.PostionCom.LocalPostion;
                    _sfmlCircle.Radius = (float)cirlce.Radius;
                    _sfmlCircle.FillColor = cirlce.Color;

                    _states.Transform = top.Owner.PostionCom.LocalToWorldMatrix;
                    _toDraw = _sfmlCircle;
                }
                else if(top is RenderTextCom)
                {
                    var text = (RenderTextCom)top;
                    _sfmlText.Position = top.Owner.PostionCom.LocalPostion;
                    _sfmlText.Font = LoadFont(text.Font.FileLocation);
                    _sfmlText.FillColor = text.Color;
                    _sfmlText.DisplayedString = text.Text;
                    
                    _states.Transform = top.Owner.PostionCom.LocalToWorldMatrix;
                   _toDraw = _sfmlText;
                }
                else if(top is DrawRectCom)
                {
                    _toDraw = top as DrawRectCom;
                }
                else
                {
                    continue;
                }

                if(top.Floating)
                {
                    _window.SetView(_window.DefaultView);
                }
                else
                {
                    _window.SetView(Camera.View);
                }

                _window.Draw(_toDraw, _states);
            }
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