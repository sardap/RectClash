using RectClash.Misc;

namespace RectClash.Game.Generation
{
	public class GrassChunkGenerator : IChunkGenerator
	{
		private int _offsetI;
		private int _offsetJ;
		private int _maxI;
		private int _maxJ;

		public void GenrateChunk(int offsetI, int offsetJ, CellInfoCom[,] cells)
		{
			_offsetI = offsetI;
			_offsetJ = offsetJ;

			_maxI = offsetI + GameConstants.CHUNK_SIZE - 1;
			_maxJ = offsetJ + GameConstants.CHUNK_SIZE - 1;

			int numberOfWalks = Utility.RandomInt(0, GameConstants.CHUNK_SIZE);

			for(int i = 0; i < numberOfWalks; i++)
				GenrateWalkMud(Utility.RandomInt(offsetI, _maxI), Utility.RandomInt(offsetJ, _maxJ), cells);
			
			if(Utility.RandomInt(0, 101) > 30)
				GenrateWalkWater(Utility.RandomInt(offsetI, _maxI), Utility.RandomInt(offsetJ, _maxJ), cells);
		}

		private void GenrateWalkWater(int i, int j, CellInfoCom[,] cells, int step = 0)
		{
			bool InRange(int val, int min, int max)
			{
				return val >= min && val <= max;
			}

			cells[i, j].Type = CellInfoCom.CellType.Water;

			int nextI;
			int nextJ;

			if(Utility.RandomBool())
			{
				nextI = i + Utility.RandomInt(-1, 2);
				nextJ = j;
			}
			else
			{
				nextI = i;
				nextJ = j + Utility.RandomInt(-1, 2);
			}
			
			var minSizeReached = !(step > 2);

			if(
				((step < GameConstants.CHUNK_SIZE && Utility.RandomInt(0, 11) > 3) || minSizeReached) &&
				(InRange(nextI, _offsetI, _maxI) && InRange(nextJ, _offsetJ, _maxJ)))
			{
				GenrateWalkWater(nextI, nextJ, cells, step++);
			}
		}

		private void GenrateWalkMud(int i, int j, CellInfoCom[,] cells, int step = 0)
		{
			int WrapIndex(int val, int min, int max)
			{
				if(val < min)
				{
					val = max;
				}
				else if(val > max)
				{
					val = min;
				}

				return val;
			}


			if(Utility.RandomBool())
			{
				cells[i, j].Type = CellInfoCom.CellType.Mud;
			}
			else
			{
				if(Utility.RandomBool())
					cells[i, j].Type = CellInfoCom.CellType.Grass;
			}

			int nextI = WrapIndex(i + Utility.RandomInt(-1, 2), _offsetI, _maxI);
			int nextJ = WrapIndex(j + Utility.RandomInt(-1, 2), _offsetJ, _maxJ);
			
			if(Utility.RandomInt(step, GameConstants.CHUNK_SIZE + 1) < GameConstants.CHUNK_SIZE)
			{
				GenrateWalkMud(nextI, nextJ, cells, step++);
			}
		}
	}
}