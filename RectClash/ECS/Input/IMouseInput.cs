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

        Vector2f ToWorldMouse(Vector2f x);

        bool IsButtonPressed(Mouse.Button button); 
    }
}