using RectClash.misc;

namespace RectClash.ECS
{
    public abstract class WindowCom : Com, IWindowCom
    {
        public abstract bool IsOpen 
        { 
            get;
        }
        
        public Vector2<int> Size 
        {
            get; 
            set;
        }

        public abstract void ProcessEvents();

        public abstract void Refresh();

        public abstract void Exit();

        
        public void Draw(IDrawable toDraw)
        {

        }

        public override void Update()
        {
            ProcessEvents();
            DrawQueue();
            Refresh();
        }

        private void DrawQueue()
        {

        }
    }
}