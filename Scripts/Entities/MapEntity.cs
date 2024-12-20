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
        this.isStatic = true;
        animationPlayer = animatedSprite2D?.GetNode<AnimationPlayer>("AnimationPlayer");
        if (animationPlayer == null)
        {
            GD.Print("WARN: MapEntity " + GetType().Name + " has no AnimationPlayer!");
        }
        
    }
    public override void OnHit(Damage damage)
    {
        animationPlayer?.Play("onHit");
        if (damage.amount < hardness)
        {
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