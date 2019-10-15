using RectClash.ECS;
using RectClash.ECS.Graphics;
using RectClash.ECS.Input;
using RectClash.game;
using RectClash.Misc;
using SFML.Graphics;

namespace RectClash.Game
{
    public class CellInputCom: Com
    {
        private CellInfoCom _cell;

        protected override void InternalStart()
        {
            _cell = Owner.GetCom<CellInfoCom>();
        }

        public override void Update()
        {
            var mouse = Engine.Instance.Mouse;

            bool InsideRect()
            {
                return Owner.PostionCom.Rect.Contains(mouse.WorldMouseX, mouse.WorldMouseY);
            }

            if(mouse.IsButtonPressed(SFML.Window.Mouse.Button.Left) && InsideRect())
            {
                switch(_cell.CurrentState)
                {
                    case CellInfoCom.State.UnSelected:
                        if(_cell.Inside.Count > 0 )
                        {
                            _cell.ChangeState(CellInfoCom.State.Selected);
                        }
                        else
                        {
                            _cell.Create();
                        }
                        break;
                    case CellInfoCom.State.InMovementRange:
                        _cell.ChangeState(CellInfoCom.State.Selected);
                        break;
                }
            }

            if(mouse.IsButtonPressed(SFML.Window.Mouse.Button.Right) && InsideRect())
            {
				_cell.ClearSelection();
            }
        }
    }
}