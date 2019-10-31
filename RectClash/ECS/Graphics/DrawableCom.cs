namespace RectClash.ECS.Graphics
{
    public class DrawableCom : Com, IDrawableCom
    {
        public bool Floating { get; set; }

        public DrawLayer Priority { get ; set ; }

		public DrawableNode DrawableNode 
		{
			get;
			private set;
		}

		public DrawableCom()
		{
			DrawableNode = new DrawableNode()
			{
				Drawable = this
			};
		}
	}
}