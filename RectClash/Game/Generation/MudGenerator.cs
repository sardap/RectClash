using RectClash.Misc;

namespace RectClash.Game.Generation
{
    public class MudGenerator : IGenerationComponent
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

		public float ProbabilityOfRunning
		{
			get; 
			set;
		}

		public void Genrate(int offsetI, int offsetJ, CellInfoCom[,] cells)
        {
            _offsetI = offsetI;
			_offsetJ = offsetJ;

			_maxI = offsetI + GameConstants.CHUNK_SIZE - 1;
			_maxJ = offsetJ + GameConstants.CHUNK_SIZE - 1;

            GenrateWalkMud(Utility.RandomInt(offsetI, _maxI), Utility.RandomInt(offsetJ, _maxJ), cells);
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

			int number = Utility.RandomInt(0, 101);

			if(number > 50)
			{
				cells[i, j].Type = CellInfoCom.CellType.Mud;
			} 
			else if(number > 25)
			{
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