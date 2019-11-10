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

		private class EnemyInAttackRange : InnerClass, IDecisionNodeCondition
		{
			public EnemyInAttackRange(RegularSoliderAICom instance) : base(instance) { }

			public bool Resolve()
			{
				var enemiesInAttackRange = Instance._gridCom
					.EntsInRange(Instance._cellInfoCom.Cords, Instance._unitInfoCom.VisionRange)
					.Where(i => Instance.IsEnemy(i.Value.First()))
					.OrderBy(i => i.Key)
					.ToList();

				if(enemiesInAttackRange.Count() <= 0)
				{
					return false;
				}

				var closestEnemyCell = enemiesInAttackRange
					.First().Value
					.First().Parent
					.GetCom<CellInfoCom>();

				Instance._closestEnemyCords = closestEnemyCell.Cords;

				var dist = Instance._gridCom.DistanceBetween(closestEnemyCell.Cords, Instance._cellInfoCom.Cords);

				if(dist > Instance._unitInfoCom.AttackRange + 1)
				{
					return false;
				}

				return true;
			}
		}


		private class EnemyVisible : InnerClass, IDecisionNodeCondition
		{
			public EnemyVisible(RegularSoliderAICom instance) : base(instance) { }

			public bool Resolve()
			{
				return Instance._closestEnemyCords != null;
			}
		}

		private class MoveThenAttackClosestEnemy : InnerClass, IAIAction
		{

			public MoveThenAttackClosestEnemy(RegularSoliderAICom instance) : base(instance) { }

			public void TakeAction()
			{
				var path = Instance._gridCom
					.AStar(Instance._cellInfoCom.Cords, (Vector2i)Instance._closestEnemyCords, false)
					.ToArray();

				var i = Instance._unitInfoCom.MovementRange - 1;

				if(path.Length <= Instance._unitInfoCom.MovementRange)
				{
					i = path.Length - 2;
				}
				
				Instance._gridCom.Move(Instance.Owner, path[i].X, path[i].Y);
				Instance._cellInfoCom.Subject.Notify(Instance.Owner, GameEvent.UNIT_MOVED);

				var dist = Instance._gridCom
					.DistanceBetween(Instance._cellInfoCom.Cords, (Vector2i)Instance._closestEnemyCords);

				if(Instance._unitInfoCom.AttackRange <= dist)
				{
					new AttackAction(Instance).TakeAction();
				}
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
				var closestEnemyCords = (Vector2i)Instance._closestEnemyCords;
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
						Condition = new EnemyInAttackRange(this),
						TrueNode = new DecisionTreeEndNode()
						{
							Action = new AttackAction(this)
						},
						FalseNode = new DecisionNode()
						{
							Condition = new EnemyVisible(this),
							TrueNode = new DecisionTreeEndNode()
							{
								Action = new MoveThenAttackClosestEnemy(this)
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
					// Cell may of changed due to movement so need to get again
					Owner.Parent.GetCom<CellInfoCom>().ChangeState(CellInfoCom.State.TurnComplete);
					_gridCom = null;
					_closestEnemyCords = null;
					_cellInfoCom = null;
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