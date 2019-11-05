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
            GenrateWalkMud(start.X, start.Y, cells, cellsInBiome, 0, genSeed);
		}

		private void GenrateWalkMud(int i, int j, CellInfoCom[,] cells, HashSet<Vector2i> cellsInBiome, int step, long seed)
		{
			int number = Utility.RandomInt(1, 102, seed + step % 1550);

			if(number > 25)
			{
				cells[i, j].Type = CellInfoCom.CellType.Mud;
			} 
			else
			{
				cells[i, j].Type = CellInfoCom.CellType.Grass;
			}

			int nextI = i + Utility.RandomInt(-1, 2, seed * (step + 1) * number >> 4);
			int nextJ = j + Utility.RandomInt(-1, 2, seed * (step + 1) * number >> 2);
			
			if(
				cellsInBiome.Contains(new Vector2i(nextI, nextJ)) && 
				(step < MinMudSteps || (Utility.RandomInt(0, 101, seed % (step + 1) - nextI + nextJ) > 5 && (step < MaxMudSteps)))
			)
			{
				GenrateWalkMud(nextI, nextJ, cells, cellsInBiome, step + 1, seed);
				return;
			}

			return;
		}

    }
}