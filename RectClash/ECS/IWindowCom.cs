using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VelcroPhysics.Dynamics;
using RectClash.misc;

namespace RectClash.ECS
{
    public interface IWindowCom : ICom
    {
        bool IsOpen { get; }
        
        Vector2<int> Size { get; set; }

        void Draw(IDrawable toDraw);

        void ProcessEvents();

        void Refresh();

        void Exit();
    }
}