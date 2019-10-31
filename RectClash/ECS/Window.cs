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
        private volatile FastPriorityQueue<DrawableNode> _drawQueue = new FastPriorityQueue<DrawableNode>(ECSConsants.MAX_DRAWABLES);

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

        public abstract void DrawDrawQueue(FastPriorityQueue<DrawableNode> drawQueue);

        public void Draw(IDrawableCom toDraw)
        {
            _drawQueue.Enqueue(toDraw.DrawableNode, 0);
        }

        public void Draw(IDrawableCom toDraw, int priority)
        {
            _drawQueue.Enqueue(toDraw.DrawableNode, priority);            
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