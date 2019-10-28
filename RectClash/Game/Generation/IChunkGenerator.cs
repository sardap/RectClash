namespace RectClash.Game.Generation
{
    public interface IChunkGenerator
    {
		void GenrateChunk(int offsetI, int offsetJ, CellInfoCom[,] cells);
    }
}