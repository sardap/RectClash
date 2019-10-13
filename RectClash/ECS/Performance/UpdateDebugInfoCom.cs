using RectClash.ECS.Graphics;

namespace RectClash.ECS.Performance
{
    public class UpdateDebugInfoCom: Com
    {
        public RenderTextCom _text;

        protected override void InternalStart()
        {
            _text = Owner.GetCom<RenderTextCom>();
        }

        public override void Update()
        {
            _text.Text = Engine.Instance.PerfMessure.AvgTick.ToString();
        }
    }
}