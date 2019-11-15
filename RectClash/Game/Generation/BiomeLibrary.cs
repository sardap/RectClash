using System.Collections.Generic;

namespace RectClash.Game.Generation
{
    public static class BiomeLibrary
    {
        public static readonly BiomeGenerator LightWoods = new BiomeGenerator()
		{
			GenerationComponents = new List<IGenerationComponent>
			{
				new FillWithTypeGenerator()
				{
					Type = CellInfoCom.CellType.Grass
				},
				new MudGenerator()
				{
					MaxMudSteps = 25,
					MinMudSteps = 1,
					NumberOfRuns = 16,
					ProbabilityOfRunning = 0.25f
				},
				new LakeGenerator()
				{
					LakeMaxSize = 5,
					LakeMinSize = 1,
					NumberOfRuns = 5,
					ProbabilityOfRunning = 0.2f
				},
				new WoodsGenerator()
				{
					TreeMaxSize = 4,
					NumberOfRuns = 6,
					ProbabilityOfRunning = 0.5f
				}
			}					
		};

		public static readonly BiomeGenerator DenseWoodsBiome = new BiomeGenerator()
		{
			GenerationComponents = new List<IGenerationComponent>
			{
				new FillWithTypeGenerator()
				{
					Type = CellInfoCom.CellType.Grass
				},
				new LakeGenerator()
				{
					LakeMaxSize = 5,
					LakeMinSize = 1,
					NumberOfRuns = 2,
					ProbabilityOfRunning = 0.2f
				},
				new WoodsGenerator()
				{
					TreeMaxSize = 8,
					NumberOfRuns = 40,
					ProbabilityOfRunning = 0.5f
				}
			}
		};

		public static readonly BiomeGenerator LakeBiome = new BiomeGenerator()
		{
			GenerationComponents = new List<IGenerationComponent>
			{
				new FillWithTypeGenerator()
				{
					Type = CellInfoCom.CellType.Grass
				},
				new MudGenerator()
				{
					MaxMudSteps = 10,
					MinMudSteps = 1,
					NumberOfRuns = 32,
					ProbabilityOfRunning = 0.25f
				},
				new LakeGenerator()
				{
					LakeMaxSize = 8,
					LakeMinSize = 3,
					NumberOfRuns = 2,
					ProbabilityOfRunning = 0.8f
				},
				new LakeGenerator()
				{
					LakeMaxSize = 2,
					LakeMinSize = 1,
					NumberOfRuns = 10,
					ProbabilityOfRunning = 0.5f
				}
			}
		};

		public static readonly BiomeGenerator DessertBiome = new BiomeGenerator()
		{
			GenerationComponents = new List<IGenerationComponent>
			{
				new FillWithTypeGenerator()
				{
					Type = CellInfoCom.CellType.Sand
				},
				new CactusGenerator()
				{
					NumberOfRuns = 40,
					ProbabilityOfRunning = 0.25f
				},
				new LakeGenerator()
				{
					LakeMaxSize = 3,
					LakeMinSize = 1,
					NumberOfRuns = 1,
					ProbabilityOfRunning = 0.2f
				}
			}
		};
    }
}