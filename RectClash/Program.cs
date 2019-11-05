using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using RectClash.ECS;
using RectClash.ECS.Graphics;
using RectClash.Game;
using RectClash.Game.Sound;
using SFML.Graphics;
using RectClash.Misc;

namespace RectClash
{
	class Program
	{
		private static Random rand = new Random();

		static long StringHash(string input)
		{
			byte[] sha256;

            using (SHA256 sha256Hash = SHA256.Create())  
            {  
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));  
  
                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();  
                for (int i = 0; i < bytes.Length; i++)  
                {  
                    builder.Append(bytes[i].ToString("x2"));  
                }  

                sha256 = Encoding.UTF8.GetBytes(builder.ToString());  
            } 

			long result = 1;
			for (int i = 0; i < sha256.Length; i ++)
			{
				if(i % 2 == 0)
					result += sha256[i] >> 4;
				else
					result *= sha256[i];
			}

			if(result < 0)
			{
				result *= -1;
			}

			return result;
		}

		static void Main(string[] args)
		{		
			int windowWidth = 1920;
			int windowHeight = 1080;

			Engine.Initialise
			(
				new SFMLComs.SFMLKeyboardInput(), 
				new SFMLComs.SFMLWindow()
				{
					Size = new SFML.System.Vector2f(windowWidth, windowHeight)
				},
				new SFMLComs.SFMLMouseInput(),
				new SFMLSoundOutput(),
				StringHash(GameConstants.SEED)
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