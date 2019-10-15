
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Priority_Queue;
using RectClash.ECS;
using RectClash.ECS.Graphics;
using RectClash.Misc;
using SFML.Graphics;
using SFML.System;

namespace RectClash.Game
{
    public class GridCom : Com, IGameObv
    {
        public enum State
        {
            NothingSelected,
            StartCellSelected,
            TargetCellSelected,
            TargetCellConf,
            ClearSelection
        }

        private static Vector2i InvalidIndex = new Vector2i(-1, -1);

        private CellInfoCom[,] _cells;

        private Vector2i _startCell;
        private Vector2i _targetCell;

        private ICollection<Vector2i> _cellsInRange;
        private List<Vector2i> _pathCells = new List<Vector2i>();

        private State _state = State.NothingSelected;

        public State CurrentState 
        {
            get => _state;
        }

        public float CellWidth { get; set; }
        public float CellHeight { get; set; }

        public GridCom()
        {
        }

        public void ChangeState(State newState)
        {
            // Check State Trans
            if(newState == State.TargetCellConf && _state != State.TargetCellSelected)
                return;

            _state = newState;

            switch(newState)
            {
                case State.NothingSelected:
                    break;
                case State.StartCellSelected:
                    var range =  Get(_startCell).Inside.First().GetCom<UnitInfoCom>().Range;
                    _cellsInRange = GetAdjacentSquaresInRange(_startCell.X, _startCell.Y, range);
					foreach(var i in _cellsInRange.Select(i => Get(i)).ToList())
                    {
                        i.ChangeState(CellInfoCom.State.InMovementRange);
                    }
                    break;
                case State.TargetCellConf:
                    var curCell = Get(_startCell);
                    var current = curCell.Inside.First();
                    curCell.Inside.Remove(current);
                    Move(current, _targetCell.X, _targetCell.Y);
                    ChangeState(State.ClearSelection);
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

        public void ClearSelection()
        {
            ChangeState(State.ClearSelection);
        }

		private ICollection<Vector2i> GetAdjacentSquaresInRange(int i, int j, int range)
		{
			var result = GetAdjacentSquaresInRange(i, j, range, new HashSet<Vector2i>());
			return result;
		}

        private ICollection<Vector2i> GetAdjacentSquaresInRange(int i, int j, int range, ICollection<Vector2i> result)
        {
			if(range <= 0)
				return result;


			foreach(var cell in GetAdjacentSquares(i, j))
			{
				GetAdjacentSquaresInRange(cell.X, cell.Y, range - 1, result);
				result.Add(cell);
			}

			return result;
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

            return result.Where(node => Get(node).SpaceAvailable);
        }

        private CellInfoCom Get(Vector2i index)
        {
            return _cells[index.X, index.Y];
        }

        public Vector2f GetGridPostion(int x, int y)
        {
            return new Vector2f(y * CellHeight, x * CellHeight);
        }

        public void AddEnt(IEnt ent, int x, int y)
        {
            Move(ent, x, y);
        }

        public void Move(IEnt ent, int x, int y)
        {
            ent.PostionCom.Postion = GetGridPostion(x, y);
            ent.PostionCom.X += CellWidth * 0.1f;
            ent.PostionCom.Y += CellHeight * 0.1f;
            ent.PostionCom.SizeX = CellWidth * 0.8f;
            ent.PostionCom.SizeY = CellHeight * 0.8f;
            _cells[x,y].Inside.Add(ent);
        }

        private bool CellSelected(Vector2i index)
        {
            switch(_state)
            {
                case State.NothingSelected:
                    _startCell = index;
                    ChangeState(State.StartCellSelected);
                    return true;
                case State.TargetCellSelected:
                    ClearPath();
                    goto case State.StartCellSelected;
                case State.StartCellSelected:
                    _targetCell = index;
                    if(_targetCell != InvalidIndex && !Get(_targetCell).SpaceAvailable)
                        return false;
                    _pathCells = AStar(_startCell, _targetCell).ToList();
                    foreach(var i in _pathCells.Select(i => Get(i)))
                    {
                        i.ChangeState(CellInfoCom.State.OnPath);
                    }
                    ChangeState(State.TargetCellSelected);
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
            foreach(var node in _cellsInRange)
            {
                Get(node).ChangeState(CellInfoCom.State.UnSelected);
            }
            _cellsInRange.Clear();
        }

        public void GenrateGrid(int gridWidth, int gridHeight, float cellWidth, float cellHeight)
        {
            CellHeight = cellHeight;
            CellWidth = cellWidth;
            _cells = new CellInfoCom[gridWidth, gridHeight];

            for(int i = 0; i < _cells.GetLength(0); i++)
            {
                for(int j = 0; j < _cells.GetLength(1); j++)
                {
                    var newCell = Engine.Instance.CreateEnt(Owner);
                    _cells[i, j] = newCell.AddCom
                    (
                        new CellInfoCom()
                        {
                            Cords = new Vector2i(i, j)
                        }
                    );
                    newCell.PostionCom.X = j * cellWidth;
                    newCell.PostionCom.Y = i * cellHeight;
                    newCell.PostionCom.SizeX = cellWidth;
                    newCell.PostionCom.SizeY = cellHeight;

                    newCell.AddCom
                    (
                        new DrawRectCom()
                        {
                            OutlineColor = new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue),
                            FillColor = Color.Black,
                            LineThickness = 1
                        }
                    );

                    newCell.AddCom
                    (
                        new CellInputCom()
                    );
                }
            }
        }

        private int GetHeuristic(Vector2i a, Vector2i b)
		{
			int result = System.Math.Abs(a.X - b.X) + System.Math.Abs(a.Y - b.Y);

			return result;
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

			actionList.Reverse();
			actionList.Remove(actionList[0]);
			actionList.Add(end);

			return actionList;
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

				foreach (var neighbor in GetAdjacentSquares(subTreeRoot.X, subTreeRoot.Y))
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

        public void OnNotify(IEnt ent, GameEvent evt)
        {
            switch(evt)
            {
                case GameEvent.CELL_SELECTED:
                    CellSelected(ent.GetCom<CellInfoCom>().Cords);
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }
    }
}