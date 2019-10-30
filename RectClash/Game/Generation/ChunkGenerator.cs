using System.Collections.Generic;
using RectClash.Misc;

namespace RectClash.Game.Generation
{
	public class ChunkGenerator
	{
		public IEnumerable<IGenerationComponent> GenerationComponents
		{
			get;
			set;
		}

		public void GenrateChunk(int offsetI, int offsetJ, CellInfoCom[,] cells)
		{
			foreach(var generator in GenerationComponents)
			{
				for(int i = 0; i < generator.NumberOfRuns; i++)
				{
					var prob = Utility.RandomDouble();
					if(prob > 1 - generator.ProbabilityOfRunning)
						generator.Genrate(offsetI, offsetJ, cells);
				}
			}
		}
	}
}