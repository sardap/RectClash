using System;
using System.Collections.Generic;
using System.Linq;
using RectClash.Misc;
using SFML.System;

namespace RectClash.Game.Generation
{
	public class WoodsGenerator : IGenerationComponent
	{
		public int TreeMaxSize
		{
			get; 
			set;
		}

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

		public WoodsGenerator()
		{
			TreeMaxSize = 1;
		}

		public void Genrate(Vector2i index, CellInfoCom[,] cells, HashSet<Vector2i> cellsInBiome, long genSeed)
		{
			var start = Utility.RandomElement(cellsInBiome, genSeed);

			var adjacent = Utility.GetAdjacentSquares(start.X, start.Y, cells);

			var startCellType = cells[start.X, start.Y].Type;

			if(adjacent.Count() == 4 && startCellType == CellInfoCom.CellType.Grass)
			{
				GenrateTree(start.X, start.Y, cells, 0, genSeed);
			}
		}

		private void GenrateTree(int cellI, int cellJ, CellInfoCom[,] cells, int step, long seed)
		{
			var adjacent = Utility.GetAdjacentSquaresSixDirections(cellI, cellJ, cells);
			adjacent = adjacent.Where(i => 
				cells[i.X, i.Y].Type == CellInfoCom.CellType.Grass || 
				cells[i.X, i.Y].Type == CellInfoCom.CellType.Mud
			).ToList();
			var numberOfLeafs = adjacent.Count();

			for(int counter = 0; counter < numberOfLeafs; counter++)
			{
				var leafCell = Utility.RandomElement(adjacent, seed + step * 1553);
				adjacent.Remove(leafCell);

				cells[leafCell.X, leafCell.Y].Type = CellInfoCom.CellType.Leaf;
			}

			cells[cellI, cellJ].Type = CellInfoCom.CellType.Wood;

			var next = Utility.RandomElement(Utility.GetAdjacentSquares(cellI, cellJ, cells), seed + step * 1554);

			if(Utility.RandomInt(0, TreeMaxSize) > step)
			{
				GenrateTree(next.X, next.Y, cells, step + 1, seed);
			}
		}
	}
}
