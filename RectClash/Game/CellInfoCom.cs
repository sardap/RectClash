using System.Collections.Generic;
using RectClash.ECS;
using RectClash.ECS.Graphics;
using RectClash.game;
using SFML.Graphics;
using SFML.System;

namespace RectClash.Game
{
    public class CellInfoCom : Com
    {
        public enum State
        {
            UnSelected,
            Selected,
            InMovementRange,
            OnPath
        }

        private static Dictionary<State, Color> _fillColorMap = new Dictionary<State, Color>()
        {
            {State.UnSelected, new Color(0, 0, 0, 0)},
            {State.Selected, Color.Red},
            {State.InMovementRange, new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue, 120)},
            {State.OnPath, new Color(125, byte.MaxValue, 125, 125)}
        };

        private Stack<State> _stateStack = new Stack<State>();

        private IList<IEnt> _inside = new List<IEnt>();
        
        private DrawRectCom _drawRectCom;

        public Subject Subject { get; set; }

        public Vector2i Cords { get; set; }

        public State CurrentState { get; set; }

        public ICollection<IEnt> Inside { get { return _inside; } }

        public bool SpaceAvailable 
        { 
            get
            {
                return Inside.Count < 1;
            }
        }

        protected override void InternalStart()
        {
            ChangeState(State.UnSelected);
            _drawRectCom = Owner.GetCom<DrawRectCom>();
        }

        public void Create()
        {
            /*
            if(_gird.CurrentState == GridCom.State.NothingSelected && SpaceAvailable)
            {
                EntFactory.CreateFootSolider(WorldCom.Instance.Owner, _gird, Cords.X, Cords.Y);
            }
            */
        }

        public void ClearSelection()
        {
            _gird.ClearSelection();
        }

        public void ChangeState(State newState)
        {
            if(newState == CurrentState)
                return;

            _stateStack.Push(newState);

            switch(newState)
            {
                case CellInfoCom.State.Selected:
                    Subject.Notify(Owner, GameEvent.CELL_SELECTED);
                    CurrentState = newState;
                    _drawRectCom.FillColor = _fillColorMap[newState];
                    break;
                case CellInfoCom.State.OnPath:
                case CellInfoCom.State.UnSelected:
                    _drawRectCom.FillColor = _fillColorMap[newState];
                    CurrentState = newState;
                    break;
                case CellInfoCom.State.InMovementRange:
                    CurrentState = newState;
                    _drawRectCom.FillColor = _fillColorMap[newState];
                    break;
                default:
                    throw new System.NotImplementedException();
            }
        }
    }
}