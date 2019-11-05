using RectClash.Misc;
using SFML.Graphics;

namespace RectClash.ECS.Graphics
{
    public class RenderTextCom : DrawableCom
    {
		private string _text;

        public PostionCom Postion { get; set; }

        public string Text 
		{ 
			get => _text;
			set
			{
				_text = value;
			}
		}

        public Font Font { get; set; }

        public Color Color { get; set; }

        protected override void InternalStart()
        {
            Postion = Owner.GetCom<PostionCom>();
        }
    }
}