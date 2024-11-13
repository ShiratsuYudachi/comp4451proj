#nullable enable
using Godot;
public interface IDamageable
{
    float Health { get; set; }
    float MaxHealth { get; }
    void ApplyDamage(long amount, Vector2? direction, Entity? source);
}