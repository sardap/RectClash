using static SFML.Window.Keyboard;
using static SFML.Window.Mouse.Button;

namespace RectClash.Game
{
	public sealed class KeyBindsAccessor
	{
		private static volatile KeyBinds instance;
	
		private static KeyBinds syncRootObject = new KeyBinds();
	
		public static KeyBinds Instance
		{
			get
			{
				if (instance == null)
				{
					lock (syncRootObject)
					{
						if (instance == null)
						{
							instance = new KeyBinds();
						}
					}

				}

				return instance;
			}
		}

		public static void SetNewKeyBinds(KeyBinds binding)
		{
			instance = binding;
		}
	}

	public class KeyBinds
	{
		public KeyBinds()
		{
			EndTurn = Key.Enter;
			ConfMove = Key.Space;
			QuitProgram = Key.Escape;

			MoveCameraRight = Key.D;
			MoveCameraLeft = Key.A;
			MoveCameraUp = Key.W;
			MoveCameraDown = Key.S;

			ToggleTeamSelection = Key.T;
			CycleUnitType = Key.Y;

		}

		public Key EndTurn { get; set; }

		public Key ConfMove { get; set; }

		public Key QuitProgram { get; set; }

		public Key MoveCameraRight { get; set; }

		public Key MoveCameraLeft { get; set; }

		public Key MoveCameraUp { get; set; }

		public Key MoveCameraDown { get; set; }

		public Key ToggleTeamSelection { get; set; }

		public Key CycleUnitType { get; set; }
	}
}