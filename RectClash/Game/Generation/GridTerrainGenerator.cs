
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RectClash.Game.Unit;
using RectClash.Misc;
using SFML.System;

namespace RectClash.Game.Generation
{
	public class GridTerrainGenerator
	{
		public WeightedList<BiomeGenerator> Biomes
		{
			get;
			set;
		}

		public GridTerrainGenerator()
		{
			Biomes = new WeightedList<BiomeGenerator>()
			{
				{ 10, BiomeLibrary.LightWoods},
				{ 10, BiomeLibrary.LakeBiome},
				{ 20, BiomeLibrary.DessertBiome},
				{ 10, BiomeLibrary.DenseWoodsBiome},
			};
		}

		public void Genrate(int chunksX, int chunksY, GridCom gridCom, long seed)
		{	
			var visited = new HashSet<Vector2i>();

			var chunkSeeds = new HashSet<long>();

			for(int i = 0; i < chunksY; i++)
			{
				for(int j = 0; j < chunksX; j++)
				{
					var chunkSeed = seed - (((j * 1081377109) + (i * 2149545691)) % 1656974148464943);
				
					Debug.Assert(!chunkSeeds.Contains(chunkSeed));
					chunkSeeds.Add(chunkSeed);

					Biomes.RandomValue(chunkSeed).
						GenrateChunk(
							i * GameConstants.CHUNK_SIZE, 
							j * GameConstants.CHUNK_SIZE, 
							gridCom.Cells, 
							visited, 
							chunkSeed
						);
				}
			}

			// Clean unassignedCells
			foreach(var cell in gridCom.Cells)
			{
				if(cell.Type == CellInfoCom.CellType.Nothing)
				{
					var adjacent = Utility.GetAdjacentSquares(cell.Cords.X, cell.Cords.Y, gridCom.Cells)
						.Where(i => gridCom.Cells[i.X, i.Y].Type != CellInfoCom.CellType.Nothing);
					
					var nextSeed = seed - cell.Cords.X + cell.Cords.Y * 4112019;

					CellInfoCom.CellType newType = CellInfoCom.CellType.Grass;

					if(adjacent.Count() > 0)
					{
						var cellToCopyIndex = Utility.RandomElement(adjacent, nextSeed);
						newType = gridCom.Cells[cellToCopyIndex.X, cellToCopyIndex.Y].Type;
					}

					cell.Type =  newType;
				}
				
				cell.ClearNotNeededChildren();
			}
		}
	}
}
