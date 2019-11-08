using System.Collections.Generic;
using SFML.System;

namespace RectClash.Game.Generation
{
	public class FillWithTypeGenerator : IGenerationComponent
	{
		public int NumberOfRuns 
		{
			get => 1;
		}

		public float ProbabilityOfRunning
		{
			get => 1f;
		}

		public CellInfoCom.CellType Type
		{
			get; 
			set;
		}

		public void Genrate(Vector2i index, CellInfoCom[,] cells, HashSet<Vector2i> cellsInBiome, long genSeed)
		{
			foreach(var cellIndex in cellsInBiome)
			{
				cells[cellIndex.X, cellIndex.Y].Type = Type;
			}
		}
	}
}