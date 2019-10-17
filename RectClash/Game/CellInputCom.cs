using RectClash.ECS;
using RectClash.ECS.Graphics;
using RectClash.ECS.Input;
using RectClash.Game;
using RectClash.Misc;
using SFML.Graphics;

namespace RectClash.Game
{
    public class CellInputCom: Com
    {
        private GameSubject _subject = new GameSubject();

        protected override void InternalStart()
        {
            _subject.AddObvs(Owner.GetCom<CellInfoCom>());
        }

        public override void Update()
        {
            var mouse = Engine.Instance.Mouse;

            bool InsideRect()
            {
                var result = Owner.PostionCom.GetWorldToLocalMatrix().TransformPoint(mouse.WorldMouseX, mouse.WorldMouseY);
                return Owner.PostionCom.Rect.Contains(result.X, result.Y);
            }

            if(mouse.IsButtonPressed(SFML.Window.Mouse.Button.Left) && InsideRect())
            {
                _subject.Notify(Owner, GameEvent.GRID_CELL_SELECTED);
            }

            if(mouse.IsButtonPressed(SFML.Window.Mouse.Button.Right) && InsideRect())
            {
                _subject.Notify(Owner, GameEvent.GRID_CLEAR_SELECTION);
            }
        }
    }
}