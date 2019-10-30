using RectClash.Misc;

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

        public void Genrate(int offsetI, int offsetJ, CellInfoCom[,] cells)
        {
            _offsetI = offsetI;
			_offsetJ = offsetJ;

			_maxI = offsetI + GameConstants.CHUNK_SIZE - 1;
			_maxJ = offsetJ + GameConstants.CHUNK_SIZE - 1;


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
			
			var minSizeReached = !(step > LakeMinSize);

			if(
				((step < LakeMaxSize && Utility.RandomInt(0, 11) > 3) || minSizeReached) &&
				(InRange(nextI, _offsetI, _maxI) && InRange(nextJ, _offsetJ, _maxJ)))
			{
				GenrateWalkWater(nextI, nextJ, cells, step++);
			}
		}

    }
}