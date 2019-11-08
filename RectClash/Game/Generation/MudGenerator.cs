using System.Collections.Generic;
using RectClash.Misc;
using SFML.System;

namespace RectClash.Game.Generation
{
    public class MudGenerator : IGenerationComponent
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

		public int MinMudSteps
		{
			get;
			set;
		}

		public int MaxMudSteps
		{
			get;
			set;
		}

		public void Genrate(Vector2i index, CellInfoCom[,] cells, HashSet<Vector2i> cellsInBiome, long genSeed)
		{
			var start = Utility.RandomElement(cellsInBiome, genSeed);
            GenrateWalkMud(start.X, start.Y, cells, cellsInBiome, 0, Utility.Randomlong(genSeed));
		}

		private void GenrateWalkMud(int i, int j, CellInfoCom[,] cells, HashSet<Vector2i> cellsInBiome, int step, long seed)
		{
			int number = Utility.RandomInt(1, 101, seed);

			if(number > 50)
			{
				cells[i, j].Type = CellInfoCom.CellType.Mud;
			} 
			else
			{
				cells[i, j].Type = CellInfoCom.CellType.Grass;
			}

			var nextSeed = Utility.Randomlong(seed) % 3523058713;

			int nextI = i + Utility.RandomInt(-1, 2, nextSeed);

			nextSeed = Utility.Randomlong(nextSeed) % 2312228717;

			int nextJ = j + Utility.RandomInt(-1, 2, nextSeed);

			nextSeed = Utility.Randomlong(nextSeed) % 2136790213;
			
			if(
				cellsInBiome.Contains(new Vector2i(nextI, nextJ)) && 
				(step < MinMudSteps || (Utility.RandomInt(0, MaxMudSteps - step, nextSeed) > 0))
			)
			{
				GenrateWalkMud(nextI, nextJ, cells, cellsInBiome, step + 1, Utility.Randomlong(seed));
				return;
			}

			return;
		}

    }
}