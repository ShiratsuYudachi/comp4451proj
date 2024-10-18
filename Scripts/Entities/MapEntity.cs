#nullable enable
using Godot;
using System;

public abstract partial class MapEntity : Entity
{
    protected int hardness = 5;
    public override void OnHit(int damage)
    {
        if (damage < hardness)
        {
            GD.Print(GetType().Name + " parried!");
            return;
        }
        base.OnHit(damage);
    }
    public override void _Process(double delta)
    {
        base._Process(delta);
    }
    public override void Die()
    {
        base.Die();
    }
}