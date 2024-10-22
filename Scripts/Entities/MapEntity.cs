#nullable enable
using Godot;
using System;

public abstract partial class MapEntity : Entity
{   

    protected AnimationPlayer? animationPlayer;

    protected int hardness = 5;

    public override void _Ready()
    {
        base._Ready();
        animationPlayer = animatedSprite2D?.GetNode<AnimationPlayer>("AnimationPlayer");
        if (animationPlayer == null)
        {
            GD.Print("WARN: MapEntity " + GetType().Name + " has no AnimationPlayer!");
        }
        
    }
    public override void OnHit(int damage)
    {
        animationPlayer?.Play("onHit");
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