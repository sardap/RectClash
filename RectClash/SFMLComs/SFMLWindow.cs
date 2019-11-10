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

		private CircleShape _sfmlCircle = new CircleShape();
		
		private SFML.Graphics.Text _sfmlText = new SFML.Graphics.Text();

		private RectangleShape _sfmlRect = new RectangleShape();

		private RenderStates _states = RenderStates.Default;

		private SFML.Graphics.Drawable _toDraw;

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
			
			if(System.Environment.GetEnvironmentVariable("RES_DIR") != null)
			{
				_workingDIR = System.Environment.GetEnvironmentVariable("RES_DIR");
			}
			else
			{
				_workingDIR = @"C:\Users\pfsar\OneDrive\Documents\GitHub\RectClash\Resources";
			}

			Camera = new Camera()
			{
				Postion = new Vector2f(Engine.Instance.Window.Size.X / 2, Engine.Instance.Window.Size.Y / 2)
			};

			_window.GainedFocus += GainedFocus;
			_window.LostFocus += LostFocus;
		}

		private void GainedFocus(object sender, EventArgs evt)
		{
			InFocus = true;
		}

		private void LostFocus(object sender, EventArgs evt)
		{
			InFocus = false;
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

		public override void DrawDrawQueue(FastPriorityQueue<DrawableNode> drawQueue)
		{
			int layer = int.MinValue;
			var vertices = new List<Vertex>();

			while(drawQueue.Count > 0)
			{
				IDrawableCom top = drawQueue.Dequeue().Drawable;

				if(top.Owner == null)
				{
					continue;
				}

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
					// if((int)top.Priority > layer)
					// {
					// 	//_states.Transform.Combine(Engine.Instance.Window.Camera.LocalToWorldTransform);
					// 	//_states.Transform.Combine(PostionCom.LocalToWorldMatrix);
					// 	_window.Draw(vertices.ToArray(), PrimitiveType.Quads);
					// 	vertices.Clear();
					// 	layer = (int)top.Priority;
					// }

					// vertices.AddRange(((DrawRectCom)top).Vertices);
					_toDraw = (DrawRectCom)top;
				}
				else
				{
					continue;
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