using System.Collections.Generic;
using System.Linq;
using RectClash.Misc;
using SFML.System;

namespace RectClash.Game.Generation
{
	public class BiomeGenerator
	{
		private Dictionary<int, int> _stepCalls = new Dictionary<int, int>();

		public IEnumerable<IGenerationComponent> GenerationComponents
		{
			get;
			set;
		}

		public void GenrateChunk(int i, int j, CellInfoCom[,] cells, HashSet<Vector2i> visited)
		{
			var cellsInBiome = GrowChunk(
				(int)(i + GameConstants.CHUNK_SIZE * 0.125), 
				(int)(j + GameConstants.CHUNK_SIZE * 0.125), 
				cells, 
				visited
			);

			var edgeCells = ChunkEdge(cellsInBiome, cells);

			foreach(var edge in edgeCells)
			{
				cellsInBiome.Add(edge);
			}

			foreach(var generator in GenerationComponents)
			{
				for(int k = 0; k < generator.NumberOfRuns; k++)
				{
					var prob = Utility.RandomDouble();
					if(prob > 1 - generator.ProbabilityOfRunning)
						generator.Genrate(new Vector2i(i, j), cells, cellsInBiome);
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
		
		private HashSet<Vector2i> ChunkEdge(HashSet<Vector2i> cellsInBiome, CellInfoCom[,] cells)
		{
			var edges = new HashSet<Vector2i>();
			
			foreach(var cell in cellsInBiome)
			{
				var adjacent = Utility.GetAdjacentSquares(cell.X, cell.Y, cells);
				if(adjacent.Any(i => !cellsInBiome.Contains(i)))
				{
					Utility.AddAll(edges, GrowEdge(cell, cellsInBiome, adjacent, cells));
				}
			}

			return edges;
		}

		private IEnumerable<Vector2i> GrowEdge(Vector2i start, HashSet<Vector2i> cellsInBiome, IEnumerable<Vector2i> adjacent, CellInfoCom[,] cells)
		{
			return GrowEdge(start, cellsInBiome, adjacent, cells, new List<Vector2i>());
		}

		private IEnumerable<Vector2i> GrowEdge(Vector2i start, HashSet<Vector2i> cellsInBiome, IEnumerable<Vector2i> adjacent, CellInfoCom[,] cells, List<Vector2i> result, int step = 0)
		{
			var filtAdj = adjacent.Where(i => !cellsInBiome.Contains(i) && !result.Contains(i));

			if(filtAdj.Count() > 0 && Utility.RandomInt(0, (int)(GameConstants.CHUNK_SIZE * 0.4) - step) > 0)
			{
				var next = Utility.RandomElement(filtAdj);
				result.Add(next);
				return GrowEdge(next, cellsInBiome, Utility.GetAdjacentSquares(next.X, next.Y, cells), cells, result, step + 1);
			}

			return result;
		}

		private HashSet<Vector2i> Grow(int i, int j, CellInfoCom[,] cells, HashSet<Vector2i> visited)
		{
			var visitQueue = new Queue<Vector2i>();
			var result = new HashSet<Vector2i>();
			var root = new Vector2i(i, j);

			visitQueue.Enqueue(root);

			while(visitQueue.Count > 0)
			{
				var current = visitQueue.Dequeue();
				visited.Add(current);
				result.Add(current);
				
				var adjacentCells = Utility.GetAdjacentSquares(current.X, current.Y, cells).Where(cell => !visited.Contains(cell)).ToList();

				var distance = Utility.DistanceBetween(root, current);

				foreach(Vector2i adjacent in adjacentCells)
				{
					if(distance < GameConstants.CHUNK_SIZE / 2 && !visitQueue.Contains(adjacent))
					{
						visitQueue.Enqueue(adjacent);
					}

				}
			}

			return result;//Grow(i, j, cells, new HashSet<Vector2i>());
		}

		private HashSet<Vector2i> Grow(int i, int j, CellInfoCom[,] cells, HashSet<Vector2i> result, int step = 0)
		{
			step++;
			result.Add(new Vector2i(i, j));

			var adjacentCells = Utility.GetAdjacentSquares(i, j, cells).Where(cell => !result.Contains(cell));

			var nextCalls = new List<Vector2i>();

			foreach(Vector2i adjacent in adjacentCells)
			{
				var random = Utility.RandomInt(0, (GameConstants.MAX_BIOME_SIZE - step) * 10000);

				if(random > 10)
				{
					if(!_stepCalls.ContainsKey(step))
					{
						_stepCalls[step] = 0;
					}

					_stepCalls[step]++;
					nextCalls.Add(new Vector2i(adjacent.X, adjacent.Y));
				}
			}

			foreach(Vector2i adjacent in nextCalls)
			{
				Grow(adjacent.X, adjacent.Y, cells, result, step);
			}

			return result;
		}
	}
}