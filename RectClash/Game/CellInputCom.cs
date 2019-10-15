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
        private Subject _subject = new Subject();

        protected override void InternalStart()
        {
            _subject.AddObvs(Owner.GetCom<CellInfoCom>());
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
                _subject.Notify(Owner, GameEvent.GRID_CELL_SELECTED);
            }

            if(mouse.IsButtonPressed(SFML.Window.Mouse.Button.Right) && InsideRect())
            {
                _subject.Notify(Owner, GameEvent.GRID_CLEAR_SELECTION);
            }
        }
    }
}