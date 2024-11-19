#nullable enable
using Godot;

public struct Damage{
    public float amount;

    public Chemistry.Element? element;
    public float? elementAmount;
    
    public Vector2? knockback; // knockback impulse
    public Entity? source;
}