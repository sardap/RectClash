using System.Collections.Generic;
using Priority_Queue;
using RectClash.ECS.Graphics;
using RectClash.Misc;
using SFML.Graphics;
using SFML.System;

namespace RectClash.ECS
{
    public abstract class Window : IWindow
    {
        private volatile SimplePriorityQueue<IDrawableCom> _drawQueue = new SimplePriorityQueue<IDrawableCom>();

        public abstract bool IsOpen 
        { 
            get;
        }

        public abstract RenderWindow RenderWindow
        {
            get;
        }

        
        public Vector2f Size 
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

        public abstract void DrawDrawQueue(SimplePriorityQueue<IDrawableCom> drawQueue);

        public void Draw(IDrawableCom toDraw)
        {
            _drawQueue.Enqueue(toDraw, 0);
        }

        public void Draw(IDrawableCom toDraw, int priority)
        {
            _drawQueue.Enqueue(toDraw, priority);            
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
                Draw(i, (int)i.Priority);
            }
        }
    }
}