using System.Collections.Generic;
using RectClash.ECS.Graphics;
using RectClash.Misc;

namespace RectClash.ECS
{
    public abstract class Window : IWindow
    {
        private volatile Queue<IDrawableCom> _drawQueue = new Queue<IDrawableCom>();

        public abstract bool IsOpen 
        { 
            get;
        }
        
        public Vector2<int> Size 
        {
            get; 
            set;
        }

        public Camera Camera { get; set; }

        public abstract void ProcessEvents();

        public abstract void Refresh();

        public abstract void Clear();

        public abstract void Exit();

        public abstract void OnStart();

        public abstract void DrawDrawQueue(Queue<IDrawableCom> drawQueue);

        public void Draw(IDrawableCom toDraw)
        {
            _drawQueue.Enqueue(toDraw);
        }

        public void Update()
        {
            ProcessEvents();
            Clear();
            DrawDrawQueue(_drawQueue);
            Refresh();
        }

        public void Draw(IEnumerable<IDrawableCom> toDraw)
        {
            foreach(var i in toDraw)
            {
                Draw(i);
            }
        }
    }
}