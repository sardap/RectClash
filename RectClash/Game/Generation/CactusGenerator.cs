using System.Collections.Generic;
using RectClash.Misc;
using SFML.System;

namespace RectClash.Game.Generation
{
	public class CactusGenerator : IGenerationComponent
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

		public void Genrate(Vector2i index, CellInfoCom[,] cells, HashSet<Vector2i> cellsInBiome, long seed)
		{
			var randomPostion = Utility.RandomElement(cellsInBiome, seed);

			cells[randomPostion.X, randomPostion.Y].Type = CellInfoCom.CellType.Cactus;
		}
	}
}