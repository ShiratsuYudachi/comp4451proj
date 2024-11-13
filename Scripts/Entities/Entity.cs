#nullable enable
using Godot;
using System;
using System.Collections.Generic;
public abstract partial class Entity : CharacterBody2D, IDamageable, IMassEntity, Chemistry.IMaterial
{
    
    // Configurable
    public int mass { get; set; } = 1;
    public float friction = 0.98f;
    protected float health = 100;
    protected float MAX_HEALTH = 100;


    // Public
    public Group group = Group.None;

    public Chemistry.Reactor reactor = new Chemistry.Reactor();
    public List<Effect> effects = new List<Effect>();

    // Internal

    protected AnimatedSprite2D? animatedSprite2D;
    [Export]
    protected PackedScene? deathEffectScene;

    
    
    public float Health
    {
        get { return health; }
        set { health = value; }
    }
    
    public Vector2 massPosition { 
        get => GlobalPosition;
        set => GlobalPosition = value;
    }
    public Vector2 massVelocity { 
        get => velocity;
        set => velocity = value;
    }

    public Vector2 velocity;

    public enum Group
    {
        Player,
        Enemy,
        Map,
        None
    }
    
    public float MaxHealth { get { return MAX_HEALTH; } }
    
    public abstract void ApplyDamage(long amout = 0L, Vector2? direction = null, Entity? source = null);
    public override void _Ready()
    {
        animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        if (animatedSprite2D == null)
        {
            GD.Print("WARN: Entity " + GetType().Name + " has no AnimatedSprite2D!");
        }
        reactor.SetMaterial(this);
    }
    public override void _Process(double delta)
    {
        Vector2 offset = new Vector2((float)(velocity.X * delta), (float)(velocity.Y * delta));
		Position += offset;
		velocity *= friction;
        foreach (Effect effect in effects.ToArray())
        {
            effect.Update(delta);
            if (effect.duration <= 0)
            {
                effects.Remove(effect);
                effect.OnRemove();
            }
        }
    }
    public virtual void OnHit(float damage)
    {
        health -= damage;
        // GD.Print(GetType().Name + " hit for " + damage + " pts. Remaining health: " + health);
        GameScene.ShowDamage(damage, GlobalPosition);
        if (health <= 0) Die();
    }
    public virtual void Die()
    {
        if (this is LivingEntity) GameScene.ShowTip(GetType().Name + " defeated!");
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

    public void onOverloaded(float elementAmount){
        // Empty implementation
        
    }

    public void onElectroCharged(float elementAmount){
        // Empty implementation
    }

    public void onSuperconduct(float elementAmount){
        // Empty implementation
    }

    public void onBurning(float elementAmount){
        // Empty implementation
        effects.Add(new BurningEffect(this, elementAmount * 10));
    }

    public void onVaporize(float elementAmount){
        // Empty implementation
    }

    public void onMelt(float elementAmount){
        // Empty implementation
    }

    public void onFreeze(float elementAmount){
        // Empty implementation
    }
}