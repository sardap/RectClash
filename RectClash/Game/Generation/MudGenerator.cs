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

		public void Genrate(int offsetI, int offsetJ, CellInfoCom[,] cells)
        {
            //GenrateWalkMud(Utility.RandomInt(offsetI, _maxI), Utility.RandomInt(offsetJ, _maxJ), cells);
			throw new System.NotImplementedException();
        }

		public void Genrate(Vector2i index, CellInfoCom[,] cells, HashSet<Vector2i> cellsInBiome)
		{
			var start = Utility.RandomElement(cellsInBiome);
            GenrateWalkMud(start.X, start.Y, cells, cellsInBiome);
		}

		private void GenrateWalkMud(int i, int j, CellInfoCom[,] cells, HashSet<Vector2i> cellsInBiome, int step = 0)
		{
			int number = Utility.RandomInt(0, 101);

			if(number > 25)
			{
				cells[i, j].Type = CellInfoCom.CellType.Mud;
			} 
			else
			{
				cells[i, j].Type = CellInfoCom.CellType.Grass;
			}

			int nextI = i + Utility.RandomInt(-1, 2);
			int nextJ = j + Utility.RandomInt(-1, 2);
			
			if(
				cellsInBiome.Contains(new Vector2i(nextI, nextJ)) && 
				(step < MinMudSteps || (Utility.RandomInt(0, 101) > 5 && (step < MaxMudSteps)))
			)
			{
				GenrateWalkMud(nextI, nextJ, cells, cellsInBiome, step + 1);
				return;
			}

			return;
		}

    }
}