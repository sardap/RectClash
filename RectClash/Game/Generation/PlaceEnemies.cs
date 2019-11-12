using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using RectClash.ECS;
using RectClash.Game.Unit;
using RectClash.Misc;
using SFML.System;

namespace RectClash.Game.Generation
{
	public class PlaceEnemies
	{

		public void GenerateEnemies(int startX, int startY, int endX, int endY, GridCom grid, int difficulty, Faction faction, long seed)
		{
			GenerateEnemies(startX, startY, endX, endY, grid, difficulty, Enum.GetValues(typeof(UnitType)).Cast<UnitType>().ToList(), faction, seed);
		}

		public void GenerateEnemies(int startX, int startY, int endX, int endY, GridCom grid, int difficulty, List<UnitType> accessibleUnits, Faction faction, long seed)
		{
			var cells = grid.Cells;

			var unitsToCreate = new Stack<UnitType>();

			var nextSeed = seed;

			while(difficulty > 0)
			{
				nextSeed = Utility.Randomlong(nextSeed);
				var nextUnitType = Utility.RandomElement(accessibleUnits, nextSeed);
				var tempDifficulty = difficulty - UnitInfoCom.DifficultyRatingForType(nextUnitType);
				if(tempDifficulty >= 0)
				{
					unitsToCreate.Push(nextUnitType);
					difficulty = tempDifficulty;
				}
				else
				{
					accessibleUnits.RemoveAll(i => 
						UnitInfoCom.DifficultyRatingForType(i) >= UnitInfoCom.DifficultyRatingForType(nextUnitType)
					);

					if(accessibleUnits.Count <= 0)
					{
						break;
					}
				}
			}

			var xSeed = (seed + ((startX * 576976759) + (endX * 104589539))) % 1828081061;
			int x = Utility.RandomInt(startX, endX, xSeed);

			var ySeed = (seed + ((startY * 2755125551) + (endY * 1920502651))) % 5291225924533335701;
			int y = Utility.RandomInt(startY, endY, ySeed);

			while(unitsToCreate.Count > 0)
			{
				if(cells[y,x].SpaceAvailable)
				{
					var nextUnit = unitsToCreate.Pop();
					EntFactory.Instance.CreateFootSolider(grid, y, x, faction, nextUnit);
				}

				xSeed = Utility.Randomlong(xSeed);
				x = Utility.RandomInt(startX, endX, xSeed);

				ySeed = Utility.Randomlong(ySeed);
				y = Utility.RandomInt(startY, endY, ySeed);

			}
		}
	}
}