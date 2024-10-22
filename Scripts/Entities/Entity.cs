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
    
    public abstract void ApplyDamage(long amout = 0L, Vector2? direction = null, Entity? source = null);
    public override void _Ready()
    {
        animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        if (animatedSprite2D == null)
        {
            GD.Print("WARN: Entity " + GetType().Name + " has no AnimatedSprite2D!");
        }
    }
    public override void _Process(double delta)
    {
        
    }
    public virtual void OnHit(int damage)
    {
        health -= damage;
        GD.Print(GetType().Name + " hit for " + damage + " pts. Remaining health: " + health);
        GameScene.ShowDamage(damage, GlobalPosition);
        if (health <= 0) Die();
    }
    public virtual void Die()
    {
        if (this is LivingEntity) TipManager.ShowTip(GetType().Name + " defeated!");
        else GD.Print(GetType().Name + " broken!");
        // 这里可以添加死亡动画、掉落物品等逻辑		
        if (deathEffectScene != null)
        {
            Node2D effect = deathEffectScene.Instantiate<Node2D>();
            GetTree().Root.AddChild(effect);
            effect.GlobalPosition = GlobalPosition;
        }
        QueueFree();
    }
}