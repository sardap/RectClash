using System;
using System.IO;
using Newtonsoft.Json;
using RectClash.ECS;
using RectClash.ECS.Graphics;
using RectClash.Game;
using RectClash.Game.Sound;
using SFML.Graphics;

namespace RectClash
{
	class Program
	{
		private static Random rand = new Random();

		static void Main(string[] args)
		{
			int windowWidth = 1280;
			int windowHeight = 720;

			Engine.Initialise
			(
				new SFMLComs.SFMLKeyboardInput(), 
				new SFMLComs.SFMLWindow()
				{
					Size = new SFML.System.Vector2f(windowWidth, windowHeight)
				},
				new SFMLComs.SFMLMouseInput(),
				new SFMLSoundOutput()
			);

			if(!File.Exists(GameConstants.KEY_BINDING_FILE))
			{
				using (var writer = new StreamWriter(GameConstants.KEY_BINDING_FILE, append: false))
				{
					writer.WriteLine(JsonConvert.SerializeObject(new KeyBinds()));
				}
			}

			using (StreamReader reader = new StreamReader(GameConstants.KEY_BINDING_FILE))
			{
				string json = reader.ReadToEnd();
				KeyBindsAccessor.SetNewKeyBinds(JsonConvert.DeserializeObject<KeyBinds>(json));
			}

			var worldEnt = EntFactory.Instance.CreateWorld();
			EntFactory.Instance.CreatePlayerInput();

			EntFactory.Instance.CreateDebugInfo();

			while(Engine.Instance.Window.IsOpen)
			{
				Engine.Instance.Step();

				Engine.Instance.UpdateWindow();
			}

		}
	}
}