using System.Collections.Generic;
using SFML.System;

namespace RectClash.Game.Generation
{
	public class GenerateEnemies : IGenerationComponent
	{
		public int NumberOfRuns
		{
			get;
			set;
		}

		public float ProbabilityOfRunning
		{
			get;
			set;
		}

		public int Difficulty
		{
			get;
			set;
		}

		public void Genrate(Vector2i index, CellInfoCom[,] cells, HashSet<Vector2i> cellsInBiome, long genSeed)
		{
			
		}
	}
}