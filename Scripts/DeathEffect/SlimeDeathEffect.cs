using Godot;
using System;

public partial class SlimeDeathEffect : Node2D
{
    public override void _Ready()
    {
        AnimatedSprite2D animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        animatedSprite2D.AnimationFinished += OnAnimationFinished;
        animatedSprite2D.Play("die");
    }
    public void OnAnimationFinished()
    {
        QueueFree();
    }
}