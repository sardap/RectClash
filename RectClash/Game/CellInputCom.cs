using RectClash.ECS;
using RectClash.ECS.Graphics;
using RectClash.ECS.Input;
using RectClash.Game;
using RectClash.Misc;
using RectClash.SFMLComs;
using SFML.Graphics;

namespace RectClash.Game
{
	public class CellInputCom: Com
	{
		private GameSubject _subject = new GameSubject();

		protected override void InternalStart()
		{
			_subject.AddObvs(Owner.GetCom<CellInfoCom>());
			Engine.Instance.Window.RenderWindow.MouseButtonPressed += MouseButtonPressed;
		}

		private RenderWindow Window
		{
			get => ((SFMLWindow)Engine.Instance.Window).RenderWindow;
		}


		private void MouseButtonPressed(object sender, SFML.Window.MouseButtonEventArgs mouse)
		{
			var trans = Owner.PostionCom.GetWorldToLocalMatrix();
			trans.Combine(Engine.Instance.Window.Camera.WorldToLocalTransform);
			var mousePos = trans.TransformPoint(mouse.X, mouse.Y);

			bool InsideRect()
			{
				var rect = Owner.PostionCom.Rect;
				
				return mousePos.X > 0 && 
					mousePos.X < 1 && 
					mousePos.Y > 0 && 
					mousePos.Y < 1;
			}

			if(InsideRect())
			{
				switch(mouse.Button)
				{
					case SFML.Window.Mouse.Button.Left:
						_subject.Notify(Owner, GameEvent.GRID_CELL_SELECTED);
						break;
					case SFML.Window.Mouse.Button.Right:
						_subject.Notify(Owner, GameEvent.GRID_CLEAR_SELECTION);
						break;
				}
			}
		}
	}
}