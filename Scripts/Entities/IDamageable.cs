#nullable enable
using Godot;
public interface IDamageable
{
    int Health { get; set; }
    int MaxHealth { get; }
    void ApplyDamage(long amount, Vector2? direction, Entity? source);
}