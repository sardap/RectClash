using System;
using RectClash.ECS;
using RectClash.Game.Unit;

namespace RectClash.Game.Perf
{
	public class DebugInputCom : Com
	{
		public Subject<string, PerfEvents> Subject { get; set; }

		protected override void InternalStart()
		{
			Engine.Instance.Window.RenderWindow.KeyReleased += Window_KeyReleased;
		}

		private void Window_KeyReleased(object sender, SFML.Window.KeyEventArgs e)
		{ 
			switch(e.Code)
			{
				case SFML.Window.Keyboard.Key.T:
					UnitType unitToCreate;
					if(EntFactory.Instance.UnitTypeToCreate == UnitType.Regular)
					{
						unitToCreate = UnitType.Heavy;
					}
					else
					{
						unitToCreate = UnitType.Regular;
					}
					EntFactory.Instance.UnitTypeToCreate = unitToCreate;
					Subject.Notify("Creating: " + Enum.GetName(unitToCreate.GetType(), unitToCreate), PerfEvents.UNIT_CREATE_SELECTION);
					break;
                case SFML.Window.Keyboard.Key.Y:
                    
                    break;
			}
		}
	}
}