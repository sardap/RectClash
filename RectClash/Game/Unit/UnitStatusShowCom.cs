using RectClash.ECS;
using RectClash.ECS.Graphics;
using SFML.Graphics;

namespace RectClash.Game.Unit
{
    public class UnitStatusShowCom : Com
    {
		private static Color _readyColor = new Color(0, 0, 0, 0);

		private static Color _notReadyColor = new Color(169, 169, 169, 125);

		private DrawRectCom _drawRectCom;

        protected override void InternalStart()
		{
			_drawRectCom = Owner.GetCom<DrawRectCom>(); 
		}

		public void Update(bool turnTaken)
		{
			_drawRectCom.FillColor = turnTaken ? _notReadyColor : _readyColor;
		}
    }
}