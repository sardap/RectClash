using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RectClash.Misc;
using SFML.System;

namespace RectClash.Game.Generation
{
	public class BiomeGenerator
	{
		private static HashSet<long> _seeds = new HashSet<long>();

		public IEnumerable<IGenerationComponent> GenerationComponents
		{
			get;
			set;
		}

		public void GenrateChunk(int chunkY, int chunkX, CellInfoCom[,] cells, HashSet<Vector2i> visited, long chunkSeed)
		{
			var cellsInBiome = GrowChunk(
				(int)(chunkY + GameConstants.CHUNK_SIZE * 0.125), 
				(int)(chunkX + GameConstants.CHUNK_SIZE * 0.125), 
				cells, 
				visited
			);

			Debug.Assert(!_seeds.Contains(chunkSeed));
			_seeds.Add(chunkSeed);

			var edgeCells = ChunkEdge(cellsInBiome, cells, chunkSeed % 100);

			foreach(var edge in edgeCells)
			{
				cellsInBiome.Add(edge);
			}

			var genSeed = chunkSeed;
			for(int i = 0; i < GenerationComponents.Count(); i++)
			{
				var generator = GenerationComponents.ElementAt(i); 
				for(int j = 0; j < generator.NumberOfRuns; j++)
				{
					var nextSeed = ((chunkSeed / ((i + 1) * 1987756522) + ((j + 1) * 1460203567)) * genSeed) % 1362291512;
					genSeed = nextSeed;

					Debug.Assert(!_seeds.Contains(nextSeed));
					_seeds.Add(nextSeed);

					var prob = Utility.RandomInt(0, 100, nextSeed) / 100f;
					if(prob > 1 - generator.ProbabilityOfRunning)
						generator.Genrate(new Vector2i(chunkY, chunkX), cells, cellsInBiome, nextSeed);
				}
			}

		}

		private HashSet<Vector2i> GrowChunk(int startI, int startJ, CellInfoCom[,] cells, HashSet<Vector2i> visited)
		{
			var result = new HashSet<Vector2i>();

			for(int i = startI; i < startI + GameConstants.CHUNK_SIZE * 0.8; i++)
			{
				for(int j = startJ; j < startJ + GameConstants.CHUNK_SIZE * 0.8; j++)
				{
					result.Add(new Vector2i(i, j));
				}
			}

			return result;
		}
		
		private HashSet<Vector2i> ChunkEdge(HashSet<Vector2i> cellsInBiome, CellInfoCom[,] cells, long biomeSeed)
		{
			var edges = new HashSet<Vector2i>();
			
			foreach(var cell in cellsInBiome)
			{
				var adjacent = Utility.GetAdjacentSquares(cell.X, cell.Y, cells);
				if(adjacent.Any(i => !cellsInBiome.Contains(i)))
				{
					var nextSeed = biomeSeed - (cell.X - cell.Y) * 512386;
					Utility.AddAll(edges, GrowEdge(cell, cellsInBiome, adjacent, cells, nextSeed));
				}
			}

			return edges;
		}

		private IEnumerable<Vector2i> GrowEdge(Vector2i start, HashSet<Vector2i> cellsInBiome, IEnumerable<Vector2i> adjacent, CellInfoCom[,] cells, long seed)
		{
			return GrowEdge(start, cellsInBiome, adjacent, cells, new List<Vector2i>(), 0, seed);
		}

		private IEnumerable<Vector2i> GrowEdge(Vector2i start, HashSet<Vector2i> cellsInBiome, IEnumerable<Vector2i> adjacent, CellInfoCom[,] cells, List<Vector2i> result, int step, long seed)
		{
			var filtAdj = adjacent.Where(i => !cellsInBiome.Contains(i) && !result.Contains(i));

			if(filtAdj.Count() > 0 && Utility.RandomInt(0, (int)(GameConstants.CHUNK_SIZE * 0.4) - step, (step + 1) * seed) > 0)
			{
				var next = Utility.RandomElement(filtAdj, (step + 1) * seed);
				result.Add(next);
				return GrowEdge(next, cellsInBiome, Utility.GetAdjacentSquares(next.X, next.Y, cells), cells, result, step + 1, seed);
			}

			return result;
		}
	}
}