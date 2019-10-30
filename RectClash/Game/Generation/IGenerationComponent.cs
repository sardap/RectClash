namespace RectClash.Game.Generation
{
    public interface IGenerationComponent
    {
        int NumberOfRuns { get; }

		float ProbabilityOfRunning { get; }

		void Genrate(int offsetI, int offsetJ, CellInfoCom[,] cells);
    }
}