using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VelcroPhysics.Dynamics;
using RectClash.Misc;
using RectClash.ECS.Graphics;
using SFML.System;

namespace RectClash.ECS
{
    public interface IWindow
    {
        bool IsOpen { get; }
        
        Vector2f Size { get; set; }

        Camera Camera { get; set; }

        void Draw(IDrawableCom toDraw);

        void Draw(IDrawableCom toDraw, int priority);

        void Draw(IEnumerable<IDrawableCom> toDraw);

        void ProcessEvents();

        void Refresh();

        void Exit();

        void Update();

        void OnStart();
    }
}