using RectClash.Misc;

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

		public void Genrate(int offsetI, int offsetJ, CellInfoCom[,] cells)
		{
			int maxI = offsetI + GameConstants.CHUNK_SIZE - 1;
			int maxJ = offsetJ + GameConstants.CHUNK_SIZE - 1;

			int i = Utility.RandomInt(offsetI, maxI);
			int j = Utility.RandomInt(offsetJ, maxJ);

			cells[i,j].Type = CellInfoCom.CellType.Cactus;
		}
	}
}