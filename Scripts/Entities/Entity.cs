#nullable enable
using Godot;
using System;
public abstract partial class Entity : CharacterBody2D, IDamageable
{
    protected AnimatedSprite2D? animatedSprite2D;
    [Export]
    protected PackedScene? deathEffectScene;
    protected int health = 100;
    public int Health
    {
        get { return health; }
        set { health = value; }
    }
    protected const int MAX_HEALTH = 100;
    public int MaxHealth { get { return MAX_HEALTH; } }
    protected float hitFlashDuration = 0.2f;
    protected float hitFlashTimer = 0f;
    public abstract void ApplyDamage(long amout = 0L, Vector2? direction = null, Entity? source = null);
    public override void _Ready()
    {
        animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        if (animatedSprite2D == null)
        {
            GD.PushError("No AnimatedSprite2D node found!");
            GetTree().Quit();
        }
    }
}