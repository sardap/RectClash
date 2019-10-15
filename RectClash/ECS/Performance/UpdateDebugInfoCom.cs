using RectClash.ECS.Graphics;

namespace RectClash.ECS.Performance
{
    public class UpdateDebugInfoCom: Com
    {
        public RenderTextCom Text { get; set; }

        protected override void InternalStart()
        {
            Text = Owner.GetCom<RenderTextCom>();
        }

        public override void Update()
        {
            string DoubleToString(double val)
            {
                return val.ToString("0.##");
            }

            var engine = Engine.Instance;
            Text.Text = "Ticks: " + DoubleToString(engine.PerfMessure.AvgTick) + 
                "\nLocalMouse(" + DoubleToString(engine.Mouse.CamMouseX) + "," + DoubleToString(engine.Mouse.CamMouseY) + ")" +
                "\nWorldMouse(" + DoubleToString(engine.Mouse.WorldMouseX) + "," + DoubleToString(engine.Mouse.WorldMouseY) + ")";
        }
    }
}