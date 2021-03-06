using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VelcroPhysics.Dynamics;
using RectClash.Misc;
using RectClash.ECS.Graphics;
using SFML.System;
using SFML.Graphics;

namespace RectClash.ECS
{
    public interface IWindow
    {
        bool IsOpen { get; }

        RenderWindow RenderWindow { get; }
        
        Vector2f Size { get; set; }

        Camera Camera { get; set; }

		bool InFocus { get; }

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