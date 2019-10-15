using SFML.System;
using SFML.Window;

namespace RectClash.ECS.Input
{
    public interface IMouseInput
    {
        float CamMouseX { get; }
        float CamMouseY { get; }
        float WorldMouseX { get; }
        float WorldMouseY { get; }

        bool IsButtonPressed(Mouse.Button button); 
    }
}