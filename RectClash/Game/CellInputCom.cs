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
			System.Console.WriteLine("RAW MOUSE PRESSED: {0} {1}", mouse.X, mouse.Y);
			
			var trans = Owner.PostionCom.GetWorldToLocalMatrix();
			trans.Combine(Engine.Instance.Window.Camera.LocalToWorldTransform.GetInverse());
			var mousePos = trans.TransformPoint(mouse.X, mouse.Y);

			System.Console.WriteLine("WORLD MOUSE PRESSED: {0} {1}", mousePos.X, mousePos.Y);

			bool InsideRect()
			{
				var rect = Owner.PostionCom.Rect;
				
				System.Console.WriteLine
				(
					"{0}: Left: {1} Right {2} Top {3} Bot {4}", 
					((Ent)Owner).Name, rect.Left, rect.Left + rect.Width, rect.Top, rect.Top + rect.Height
				);

				System.Console.WriteLine
				(
					"{0}: Left: {1} Right {2} Top {3} Bot {4}", 
					((Ent)Owner).Name, mousePos.X > 0, mousePos.X < 1, mousePos.Y > 0, mousePos.Y < 1
				);

				
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