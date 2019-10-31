namespace RectClash.Game.Generation
{
	public class SandGenerator : IGenerationComponent
	{
		public int NumberOfRuns 
		{
			get => 1;
		}

		public float ProbabilityOfRunning
		{
			get => 1f;
		}

		public void Genrate(int offsetI, int offsetJ, CellInfoCom[,] cells)
		{
			for(int i = offsetI; i < offsetI + GameConstants.CHUNK_SIZE; i++)
			{
				for(int j = offsetJ; j < offsetJ + GameConstants.CHUNK_SIZE; j++)
				{
					cells[i,j].Type = CellInfoCom.CellType.Sand;
				}
			}
		}
	}
}