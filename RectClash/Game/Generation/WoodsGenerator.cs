using System;
using System.Collections.Generic;
using System.Linq;
using RectClash.Misc;

namespace RectClash.Game.Generation
{
	public class WoodsGenerator : IGenerationComponent
	{
		private readonly HashSet<Tuple<int, int>> _visitedCells = new HashSet<Tuple<int, int>>();

		private int _offsetI;
		private int _offsetJ;
		private int _maxI;
		private int _maxJ;

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

		public void Genrate(int offsetI, int offsetJ, CellInfoCom[,] cells)
		{
            _offsetI = offsetI;
			_offsetJ = offsetJ;

			_maxI = offsetI + GameConstants.CHUNK_SIZE - 1;
			_maxJ = offsetJ + GameConstants.CHUNK_SIZE - 1;


			int i = Utility.RandomInt(_offsetI, _maxI);
			int j = Utility.RandomInt(_offsetJ, _maxJ);

			var adjacent = Utility.GetAdjacentSquares(i, j, cells);

			if(adjacent.Count() == 4 && cells[i, j].Type == CellInfoCom.CellType.Grass)
			{
				GenrateTree(i, j, cells);
			}
		}

		private void GenrateTree(int cellI, int cellJ, CellInfoCom[,] cells, int step = 0)
		{
			var adjacent = Utility.GetAdjacentSquaresSixDirections(cellI, cellJ, cells);
			adjacent = adjacent.Where(i => 
				cells[i.Item1, i.Item2].Type == CellInfoCom.CellType.Grass || 
				cells[i.Item1, i.Item2].Type == CellInfoCom.CellType.Mud
			).ToList();
			var numberOfLeafs = adjacent.Count();

			for(int counter = 0; counter < numberOfLeafs; counter++)
			{
				var leafCell = Utility.RandomElement(adjacent);
				adjacent.Remove(leafCell);

				cells[leafCell.Item1, leafCell.Item2].Type = CellInfoCom.CellType.Leaf;
			}

			cells[cellI, cellJ].Type = CellInfoCom.CellType.Wood;

			var next = Utility.RandomElement(Utility.GetAdjacentSquares(cellI, cellJ, cells));

			if(Utility.RandomInt(0, TreeMaxSize) > step)
			{
				GenrateTree(next.Item1, next.Item2, cells, ++step);
			}
		}
	}
}
