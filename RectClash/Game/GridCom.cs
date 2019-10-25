using System.Collections.Generic;
using System.Linq;
using Priority_Queue;
using RectClash.ECS;
using RectClash.Game.Unit;
using SFML.System;

namespace RectClash.Game
{
	public class GridCom : Com, IGameObv
	{
		public enum State
		{
			NothingSelected,
			StartCellSelected,
			MoveTargetCellSelected,
			AttackTargetCellSelected,
			ClearSelection
		}

		private static Vector2i InvalidIndex = new Vector2i(-1, -1);

		private CellInfoCom[,] _cells;

		private Vector2i _startCell;

		private Vector2i _targetCell;

		private IEnt _attackTarget;

		private ICollection<Vector2i> _cellsInRange;

		private List<Vector2i> _pathCells = new List<Vector2i>();

		private State _state = State.NothingSelected;

		public State CurrentState 
		{
			get => _state;
		}

		public float CellWidth { get; set; }

		public float CellHeight { get; set; }

		public GameSubject Subject { get; set; }

		public TurnHandlerCom TurnHandler { get; set; }

		public GridCom()
		{
		}

		private void ChangeState(State newState)
		{
			_state = newState;

			switch(newState)
			{
				case State.NothingSelected:
					break;

				case State.StartCellSelected:
					var UnitInfoCom = Get(_startCell).Inside.First().GetCom<UnitInfoCom>();
					var range = UnitInfoCom.Range;
					var faction = UnitInfoCom.Faction;

					_cellsInRange = GetAdjacentSquaresInRange(_startCell.X, _startCell.Y, range);
					_cellsInRange.Remove(_startCell);
					
					Get(_startCell).ChangeState(CellInfoCom.State.Selected);
					Get(_startCell).Subject.Notify(Owner, GameEvent.UNIT_SELECTED);

					foreach(var i in _cellsInRange.Select(i => Get(i)).ToList())
					{
						if(i.SpaceAvailable)
						{
							i.ChangeState(CellInfoCom.State.InMovementRange);
						}
						else if (i.Inside.Count() > 0)
						{
							var first = i.Inside.First();
							if(first.Tags.Contains(Tags.FOOT_SOILDER) && first.GetCom<UnitInfoCom>().Faction != faction)
							{
								i.ChangeState(CellInfoCom.State.CanAttack);
							}
						}
					}
					break;

				case State.ClearSelection:
					ClearPath();
					ClearCellsInRange();
					if(_startCell != InvalidIndex)
						Get(_startCell).ChangeState(CellInfoCom.State.UnSelected);
					if(_targetCell != InvalidIndex)
						Get(_targetCell).ChangeState(CellInfoCom.State.UnSelected);
					_startCell = InvalidIndex;
					_targetCell = InvalidIndex;
					ChangeState(State.NothingSelected);
					break;
			}
		}

		private ICollection<Vector2i> GetAdjacentSquaresInRange(int i, int j, int range)
		{
			var result = GetAdjacentSquaresInRange(i, j, range, new HashSet<Vector2i>());
			return result;
		}

		private ICollection<Vector2i> GetAdjacentSquaresInRange(int i, int j, int range, ICollection<Vector2i> result)
		{
			if(range < 0)
				return result;

			result.Add(new Vector2i(i, j));

			var adjacentSquares = GetAdjacentSquares(i, j).Where(node => Get(node).Selectable);

			foreach(var cell in adjacentSquares.Where(node => !Get(node).SpaceAvailable))
			{
				result.Add(cell);
			}

			foreach(var cell in adjacentSquares.Where(node => Get(node).SpaceAvailable))
			{
				GetAdjacentSquaresInRange(cell.X, cell.Y, range - _cells[i, j].MovementCost, result);
			}

			return result;
		}

		private IEnumerable<Vector2i> GetAdjacentSquares(Vector2i index)
		{
			return GetAdjacentSquares(index.X, index.Y);
		}


		private IEnumerable<Vector2i> GetAdjacentSquares(int i, int j)
		{
			var result = new List<Vector2i>();

			if(i + 1 < _cells.GetLength(0))
			{
				result.Add(new Vector2i(i + 1, j));
			}

			if(i - 1 >= 0)
			{
				result.Add(new Vector2i(i - 1, j));
			}

			if(j + 1 < _cells.GetLength(1))
			{
				result.Add(new Vector2i(i, j + 1));
			}

			if(j - 1 >= 0)
			{
				result.Add(new Vector2i(i, j - 1));
			}

			return result;
		}

		private CellInfoCom Get(Vector2i index)
		{
			return _cells[index.X, index.Y];
		}

		private bool CellSelected(CellInfoCom cell)
		{
			Vector2i index = cell.Cords;

			switch(_state)
			{
				case State.NothingSelected:
					if( 
						!(cell.CurrentState == CellInfoCom.State.UnSelected && cell.Inside.Count() > 0) &&
						!(cell.CurrentState == CellInfoCom.State.InMovementRange) &&
						!(cell.CurrentState == CellInfoCom.State.TurnComplete)
					)
					{
						EntFactory.Instance.CreateFootSolider(this, index.X, index.Y);
						break;
					}

					var inside = Get(index).Inside.First().GetCom<UnitInfoCom>();
					if(inside.Faction != TurnHandler.Faction || inside.TurnTaken)
					{
						break;
					}

					_startCell = index;
					ChangeState(State.StartCellSelected);
					return true;

				case State.MoveTargetCellSelected:
					ClearPath();
					goto case State.StartCellSelected;
				case State.StartCellSelected:
					if(
						cell.CurrentState != CellInfoCom.State.InMovementRange && 
						cell.CurrentState != CellInfoCom.State.CanAttack
					)
						break;

					_targetCell = index;

					if(cell.CurrentState == CellInfoCom.State.InMovementRange)
					{
						_pathCells = AStar(_startCell, _targetCell).ToList();
						foreach(var i in _pathCells.Select(i => Get(i)))
						{
							i.ChangeState(CellInfoCom.State.OnPath);
						}

						ChangeState(State.MoveTargetCellSelected);							
					}
					else if(cell.CurrentState == CellInfoCom.State.CanAttack)
					{
						var target = Get(_targetCell).Inside.First();
						var start = Get(_startCell).Inside.First();
						
						var adjacentSquares = GetAdjacentSquares(_targetCell);

						if(!adjacentSquares.Contains(_startCell))
						{
							_pathCells = AStar(_startCell, adjacentSquares.Where(i => Get(i).SpaceAvailable)).ToList();
							foreach(var i in _pathCells.Select(i => Get(i)))
							{
								i.ChangeState(CellInfoCom.State.OnPath);
							}

							_targetCell = _pathCells.Last();	
						}
						else
						{
							_targetCell = _startCell;
						}

						_attackTarget = target;

						ChangeState(State.AttackTargetCellSelected);
					}
					else
					{
						throw new System.NotImplementedException();
					}

					
					return true;
			}

			return false;
		}

		private void ClearPath()
		{
			_pathCells.ForEach(i => Get(i).ChangeState(CellInfoCom.State.InMovementRange));
			_pathCells.Clear();
		}

		private void ClearCellsInRange()
		{
			if(_cellsInRange == null)
				return;

			foreach(var node in _cellsInRange)
			{
				Get(node).ChangeState(CellInfoCom.State.UnSelected);
			}
			_cellsInRange.Clear();
		}    

		private double GetHeuristic(Vector2i aPos, Vector2i bPos)
		{
			var dx = System.Math.Abs(aPos.X - bPos.X);
			var dy = System.Math.Abs(aPos.Y - bPos.Y);

			return Get(aPos).MovementCost * (dx + dy);
		}

		private double DistanceBetween(Vector2i aPos, Vector2i bPos)
		{
			return System.Math.Sqrt(System.Math.Pow(bPos.X - aPos.X, 2) + System.Math.Pow(bPos.Y - aPos.Y, 2));
		}

		private List<Vector2i> ConsturctPath(Vector2i state, Dictionary<Vector2i, Vector2i?> meta)
		{
			var actionList = new List<Vector2i>();
			var end = state;

			while (true)
			{
				var row = meta[state];
				if (row != null)
				{
					state = (Vector2i)row;
					actionList.Add((Vector2i)row);
				}
				else
				{
					break;
				}
			}

			if(actionList.Count == 0)
			{
				return null;
			}

			actionList.Reverse();
			actionList.Remove(actionList[0]);
			actionList.Add(end);

			return actionList;
		}

		private IEnumerable<Vector2i> AStar(Vector2i start, IEnumerable<Vector2i> goals)
		{
			IEnumerable<Vector2i> result = null;
			foreach(var goal in goals)
			{
				var path = AStar(start, goal);
				if(result == null || (path != null && path.Count() < result.Count()))
				{
					result = path;
				}
			}

			return result;
		}

		
		private IEnumerable<Vector2i> AStar(Vector2i start, Vector2i goal)
		{
			//Holds the qeue of nodes to check which is sorted by there fscore
			var queue = new SimplePriorityQueue<Vector2i, double>();
			queue.Enqueue(start, 0);
			//Holds each visted node
			var closedSet = new HashSet<Vector2i> { };
			//Used to find the path
			var meta = new Dictionary<Vector2i, Vector2i?>();
			// Used to create the fscore
			var gScore = new Dictionary<Vector2i, double>
			{
				{ start, 0 }
			};


			var root = start;
			meta[root] = null;

			while (queue.Count != 0)
			{
				var subTreeRoot = queue.Dequeue();

				//Breaks if the goal has been found
				if (subTreeRoot == goal)
				{
					return ConsturctPath(subTreeRoot, meta);
				}

				var neighbors = GetAdjacentSquares(subTreeRoot.X, subTreeRoot.Y).
					Where(i => Get(i).CurrentState == CellInfoCom.State.InMovementRange);

				foreach (var neighbor in neighbors)
				{
					//If the node has been visted skip
					if (closedSet.Contains(neighbor))
					{
						continue;
					}
					//if the node is a worse path skip
					double tentativeGScore = gScore[subTreeRoot] + DistanceBetween(subTreeRoot, neighbor);
					if (gScore.ContainsKey(neighbor) && tentativeGScore >= gScore[neighbor])
					{
						continue;
					}
					
					gScore[neighbor] = tentativeGScore;

					if (!queue.Contains(neighbor))
					{
						meta[neighbor] = subTreeRoot;
						//Adds the node to the qeue with it's fscore
						queue.Enqueue(neighbor, gScore[neighbor] + GetHeuristic(neighbor, goal));
					}

				}

				//adds that the node has been visted
				closedSet.Add(subTreeRoot);
			}

			return ConsturctPath(start, meta);
		}

		private void ApplyMove()
		{
			var curCell = Get(_startCell);
			var current = curCell.Inside.First();
			var targetCellIndex = _targetCell;
			Move(current, targetCellIndex.X, targetCellIndex.Y);
			ChangeState(State.ClearSelection);

			Get(targetCellIndex).Subject.Notify(Get(targetCellIndex).Owner, GameEvent.UNIT_MOVED);
			Get(targetCellIndex).ChangeState(CellInfoCom.State.TurnComplete);
		}

		private void ApplyAttack()
		{
			var curCell = Get(_startCell);
			var current = curCell.Inside.First();
			var targetCellIndex = _targetCell;
			Move(current, targetCellIndex.X, targetCellIndex.Y);
			ChangeState(State.ClearSelection);

			Get(targetCellIndex).Subject.Notify(_attackTarget, GameEvent.ATTACK_TARGET);
			_attackTarget = null;
			Get(targetCellIndex).ChangeState(CellInfoCom.State.TurnComplete);
		}

		public void AddEnt(IEnt ent, int x, int y)
		{
			Move(ent, x, y);
		}

		private void Move(IEnt ent, int x, int y)
		{
			if(ent.Parent.Tags.Contains(Tags.GRID_CELL))
			{
				ent.Parent.GetCom<CellInfoCom>().Subject.RemoveObv(ent.GetCom<UnitActionContCom>());
			}

			ent.ChangeParent(_cells[x,y].Owner);
			_cells[x,y].Subject.AddObv(ent.GetCom<UnitActionContCom>());
		}

		public void GenrateGrid(int gridWidth, int gridHeight, float cellWidth, float cellHeight)
		{
			CellHeight = cellHeight;
			CellWidth = cellWidth;
			_cells = new CellInfoCom[gridWidth, gridHeight];
			Owner.PostionCom.LocalScale = new Vector2f(cellWidth, cellHeight);

			for(int i = 0; i < _cells.GetLength(0); i++)
			{
				for(int j = 0; j < _cells.GetLength(1); j++)
				{
					_cells[i, j] = EntFactory.Instance.CreateCell(this, i, j, CellWidth, CellHeight).GetCom<CellInfoCom>();
				}
			}
		}

		private void OnTurnEnd()
		{
			foreach(var cell in _cells)
			{
				if(cell.Inside.Count() > 0)
				{
					var unitCom = cell.Inside.First().GetCom<UnitInfoCom>();
					if(unitCom.Faction == TurnHandler.Faction)
					{
						unitCom.TurnTaken = false;
						cell.ChangeState(CellInfoCom.State.UnSelected);
					}
				}
			}
			
			ChangeState(State.ClearSelection);
		}

		public void OnNotify(IEnt ent, GameEvent evt)
		{
			switch(evt)
			{
				case GameEvent.GRID_CELL_SELECTED:
					var cell = ent.GetCom<CellInfoCom>();
					if(CellSelected(cell))
					{
						cell.ChangeState(CellInfoCom.State.Selected);

						foreach(var unit in cell.Inside)
						{
							unit.GetCom<UnitActionContCom>();
						}
					}
					break;
				case GameEvent.CREATE_FOOTSOLIDER:
					if(CurrentState != State.NothingSelected)
						break;
					var cords = ent.GetCom<CellInfoCom>().Cords;
					EntFactory.Instance.CreateFootSolider(this, cords.X, cords.Y);
					break;
				case GameEvent.GRID_MOVE_CONF:
					if(_state == State.MoveTargetCellSelected)
						ApplyMove();
					else if(_state == State.AttackTargetCellSelected)
						ApplyAttack();
					break;
				case GameEvent.TURN_END:
					OnTurnEnd();
					break;
				case GameEvent.GRID_CLEAR_SELECTION:
					ChangeState(State.ClearSelection);
					break;
			}
		}
	}
}