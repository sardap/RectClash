using System.Collections.Generic;
using RectClash.ECS;
using RectClash.ECS.Graphics;
using RectClash.game;
using SFML.Graphics;
using SFML.System;

namespace RectClash.Game
{
    public class CellInfoCom : Com, IGameObv
    {
        public enum CellType
        {
            Dirt,
            Mud,
            Water
        }

        public enum State
        {
            StartingSate,
            UnSelected,
            Selected,
            InMovementRange,
            OnPath
        }

        private static Dictionary<State, Color> _fillColorMap = new Dictionary<State, Color>()
        {
            {State.UnSelected, new Color(0, 0, 0, 0)},
            {State.Selected, new Color(byte.MaxValue, 0, 0, 120)},
            {State.InMovementRange, new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, 120)},
            {State.OnPath, new Color(0, byte.MaxValue, 0, byte.MaxValue)}
        };


        private static Dictionary<CellType, Color> _backgroundColorMap = new Dictionary<CellType, Color>()
        {
            {CellType.Dirt, Color.Black},
            {CellType.Water, Color.Blue},
            {CellType.Mud, new Color(165, 42, 42)}
        };

        private Stack<State> _stateStack = new Stack<State>();

        private IList<IEnt> _inside = new List<IEnt>();
        
        private DrawRectCom _drawRectCom;

        public CellType Type { get; set; }

        public Subject Subject { get; set; }

        public Vector2i Cords { get; set; }

        public State CurrentState { get; set; }

        public ICollection<IEnt> Inside { get { return _inside; } }

        public DrawRectCom Background { get; set; }


        public int MovementCost
        {
            get
            {
                switch(Type)
                {
                    case CellType.Mud:
                        return 3;
                    default:
                        return 1;
                }
            }
            
        }

        public bool SpaceAvailable 
        { 
            get
            {
                return (Type == CellType.Dirt || Type == CellType.Mud) && Inside.Count <= 0;
            }
        }

        protected override void InternalStart()
        {
            _drawRectCom = Owner.GetCom<DrawRectCom>();
            ChangeState(State.UnSelected);
        }

        public void ChangeState(State newState)
        {
            if(newState == CurrentState)
                return;

            _stateStack.Push(newState);

            switch(newState)
            {
                case CellInfoCom.State.Selected:
                    CurrentState = newState;
                    _drawRectCom.FillColor = _fillColorMap[newState];
                    break;
                case CellInfoCom.State.UnSelected:
                case CellInfoCom.State.OnPath:
                    CurrentState = newState;
                    _drawRectCom.FillColor = _fillColorMap[newState];
                    break;
                case CellInfoCom.State.InMovementRange:
                    CurrentState = newState;
                    _drawRectCom.FillColor = _fillColorMap[newState];
                    break;
                default:
                    throw new System.NotImplementedException();
            }
            
            Background.FillColor = _backgroundColorMap[Type];
        }

        public void OnNotify(IEnt ent, GameEvent evt)
        {
            switch(evt)
            {
                case GameEvent.GRID_CELL_SELECTED:
                    Subject.Notify(Owner, GameEvent.GRID_CELL_SELECTED);
                    break;
                case GameEvent.GRID_CLEAR_SELECTION:
                    Subject.Notify(Owner, GameEvent.GRID_CLEAR_SELECTION);
                    break;
            }
        }
    }
}