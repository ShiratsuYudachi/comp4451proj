#nullable enable
using Godot;
public interface IMassEntity
{
    int mass { get; set; }
    Vector2 massPosition { get; set; }
    Vector2 massVelocity { get; set; }
}