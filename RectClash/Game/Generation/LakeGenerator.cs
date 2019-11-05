using System.Collections.Generic;
using RectClash.Misc;
using SFML.System;

namespace RectClash.Game.Generation
{
    public class LakeGenerator : IGenerationComponent
    {
		private int _offsetI;
		private int _offsetJ;
		private int _maxI;
		private int _maxJ;

        public int NumberOfRuns
		{
			get;
			set;
		}
		
		public int LakeMaxSize 
		{
			get;
			set;
		}

		public int LakeMinSize
		{
			get;
			set;
		}

		public float ProbabilityOfRunning
		{
			get;
			set;
		}

		public LakeGenerator()
		{
			LakeMaxSize = GameConstants.CHUNK_SIZE;
			LakeMinSize = 2;
		}

		public void Genrate(Vector2i index, CellInfoCom[,] cells, HashSet<Vector2i> cellsInBiome, long genSeed)
		{
			var start = Utility.RandomElement(cellsInBiome, genSeed);
            GenrateWalkWater(start.X, start.Y, cells, cellsInBiome, 0, genSeed);
		}

        private void GenrateWalkWater(int i, int j, CellInfoCom[,] cells, HashSet<Vector2i> cellsInBiome, int step, long genSeed)
		{
			cells[i, j].Type = CellInfoCom.CellType.Water;

			int nextI;
			int nextJ;

			nextI = i + Utility.RandomInt(-1, 2);
			nextJ = j;

			if(
				(step < LakeMinSize || Utility.RandomInt(0, LakeMaxSize - step) > 0) && 
				cellsInBiome.Contains(new Vector2i(nextI, nextJ))
			)
			{
				GenrateWalkWater(nextI, nextJ, cells, cellsInBiome, step + 1, genSeed);

			}
			
			nextI = i;
			nextJ = j + Utility.RandomInt(-1, 2);

			if(
				(step < LakeMinSize || Utility.RandomInt(0, LakeMaxSize - step) > 0) && 
				cellsInBiome.Contains(new Vector2i(nextI, nextJ))
			)
			{
				GenrateWalkWater(nextI, nextJ, cells, cellsInBiome, step + 1, genSeed);
			}
		}
	}
}