using System.Collections.Generic;
using System.Linq;
using RectClash.ECS;
using RectClash.Game.Unit;
using RectClash.Misc;
using SFML.System;

namespace RectClash.Game.AI
{
    public class RegularSoliderAICom : Com, IGameObv
    {
		private enum State
		{
			Seraching
		}

		private class InnerClass
		{
			protected RegularSoliderAICom Instance
			{
				get;
				private set;
			}

			public InnerClass(RegularSoliderAICom instance)
			{
				Instance = instance;
			}
		}

		private class AdjacentCellFree : InnerClass, IDecisionNodeCondition
		{
			public AdjacentCellFree(RegularSoliderAICom instance) : base(instance) { }

			public bool Resolve()
			{
				var adjacentCells = Utility.GetAdjacentSquares
				(
					Instance._cellInfoCom.Cords.X,
					Instance._cellInfoCom.Cords.Y,
					Instance._gridCom.Cells
				).Where(i => Instance._gridCom.Cells[i.Y, i.X].SpaceAvailable);

				return adjacentCells.Count() > 0;
			}
		}

		private class EnemyAdjacent : InnerClass, IDecisionNodeCondition
		{
			public EnemyAdjacent(RegularSoliderAICom instance) : base(instance) { }

			public bool Resolve()
			{
				return Utility.GetAdjacentSquares(
					Instance._cellInfoCom.Cords.X, 
					Instance._cellInfoCom.Cords.Y, 
					Instance._gridCom.Cells
				).Any(i => Instance._gridCom.Cells[i.X, i.Y].Inside.Any(j => Instance.IsEnemy(j)));
			}
		}


		private class EnemyVisible : InnerClass, IDecisionNodeCondition
		{
			public EnemyVisible(RegularSoliderAICom instance) : base(instance) { }

			public bool Resolve()
			{
				var visibleEnts = Instance._gridCom.EntsInRange(Instance._cellInfoCom.Cords, 5);

				if(visibleEnts.Keys.Count > 0)
				{
					var sortedEntsKeys = visibleEnts.Keys.OrderBy(i => i);

					foreach(double key in sortedEntsKeys)
					{
						foreach(IEnt ent in visibleEnts[key])
						{
							if(ent.Tags.Contains(Tags.UNIT))
							{
								var otherUnitCom = ent.GetCom<UnitInfoCom>();
								if(otherUnitCom.Faction != Instance._unitInfoCom.Faction)
								{
									Instance._closestEnemyCords = ent.Parent.GetCom<CellInfoCom>().Cords;
									return true;
								}
							}
						}
					}
				}

				return false;
			}
		}

		private class MoveTowardsClosestEnemy : InnerClass, IAIAction
		{

			public MoveTowardsClosestEnemy(RegularSoliderAICom instance) : base(instance) { }

			public void TakeAction()
			{
				var path = Instance._gridCom.AStar(Instance._cellInfoCom.Cords, (Vector2i)Instance._closestEnemyCords, false);
				Instance._gridCom.Move(Instance.Owner, path.First().X, path.First().Y);
			}
		}

		private class DoNothingAction : InnerClass, IAIAction
		{
			public DoNothingAction(RegularSoliderAICom instance) : base(instance) { }

			public void TakeAction()
			{
			}
		}

		private class AttackAction : InnerClass, IAIAction
		{
			public AttackAction(RegularSoliderAICom instance) : base(instance) { }

			public void TakeAction()
			{
				var closestEnemyCords = Instance.ClosestEnemy().Parent.GetCom<CellInfoCom>().Cords;
				var enemy = Instance._gridCom.Cells[closestEnemyCords.X, closestEnemyCords.Y].Inside.First();
				Instance._cellInfoCom.Subject.Notify(enemy, GameEvent.ATTACK_TARGET);
			}
		}		

		private Dictionary<State, IDecisionTreeNode> _decisionTrees;

		private State _currentState;

		private CellInfoCom _cellInfoCom;

		private UnitInfoCom _unitInfoCom;

		private GridCom _gridCom;

		private Vector2i? _closestEnemyCords;

		protected override void InternalStart()
		{
			_unitInfoCom = Owner.GetCom<UnitInfoCom>();

			_currentState = State.Seraching;

			_decisionTrees = new Dictionary<State, IDecisionTreeNode>()
			{
				{
					State.Seraching,
					new DecisionNode()
					{
						Condition = new EnemyAdjacent(this),
						TrueNode = new DecisionTreeEndNode()
						{
							Action = new AttackAction(this)
						},
						FalseNode = new DecisionNode()
						{
							Condition = new EnemyVisible(this),
							TrueNode = new DecisionTreeEndNode()
							{
								Action = new MoveTowardsClosestEnemy(this)
							},
							FalseNode = new DecisionTreeEndNode()
							{
								Action = new DoNothingAction(this)
							}
						}
					}
				}
			};
		}
		
		public void OnNotify(IEnt ent, GameEvent evt)
		{
			switch(evt)
			{
				case GameEvent.AI_TAKE_TURN:
				{
					_cellInfoCom = Owner.Parent.GetCom<CellInfoCom>();
					_gridCom = ent.GetCom<GridCom>();
					var action = _decisionTrees[_currentState].GetAction();
					action.TakeAction();
					_gridCom = null;
					break;
				}
			}
		}

		private IEnt ClosestEnemy()
		{
			var adjacentCells = Utility.GetAdjacentSquares(_cellInfoCom.Cords.X, _cellInfoCom.Cords.Y, _gridCom.Cells);
			var adjacentEnemy = adjacentCells.Where(i => _gridCom.Cells[i.X, i.Y].Inside.Any(j => j.Tags.Contains(Tags.UNIT)));
			return _gridCom.Cells[adjacentEnemy.First().X, adjacentEnemy.First().Y].Inside.First();
		}

		private bool IsEnemy(IEnt other)
		{
			return other.Tags.Contains(Tags.UNIT) && other.GetCom<UnitInfoCom>().Faction != _unitInfoCom.Faction;
		}
	}
}