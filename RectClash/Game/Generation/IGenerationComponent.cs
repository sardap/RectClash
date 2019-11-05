using System.Collections.Generic;
using SFML.System;

namespace RectClash.Game.Generation
{
    public interface IGenerationComponent
    {
        int NumberOfRuns { get; }

		float ProbabilityOfRunning { get; }

		void Genrate(Vector2i index, CellInfoCom[,] cells, HashSet<Vector2i> cellsInBiome, long genSeed);
    }
}