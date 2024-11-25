#nullable enable
using Godot;
using System;
using System.Collections.Generic;
using Chemistry;
public abstract partial class Entity : CharacterBody2D, IMassEntity, IMaterial
{
    
    // Configurable
    public int mass { get; set; } = 1; // multiplier for handling ApplyMotion
    public float frictionMultiplier = 1f;
    protected float health = 100;
    protected float MAX_HEALTH = 100;


    // Public
    public Group group = Group.None;

    public bool isStatic = false;

    public Reactor reactor = new Reactor();
    public List<Effect> effects = new List<Effect>();
    
    private float nextDamageMultiplier = 1;

    // Internal

    protected AnimatedSprite2D? animatedSprite2D;
    [Export]
    protected PackedScene? deathEffectScene;

    public float Health
    {
        get { return health; }
        set { health = value; }
    }

    public Vector2 massPosition
    {
        get => GlobalPosition;
        set => GlobalPosition = value;
    }
    public Vector2 massVelocity
    {
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

    public override void _Ready()
    {
        animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        if (animatedSprite2D == null)
        {
            GD.Print("WARN: Entity " + GetType().Name + " has no AnimatedSprite2D!");
        }
        reactor.SetMaterial(this);

        PackedScene? scene = ResourceManager.GetScene(SceneResourceType.ElementDisplay);
        if (scene != null)
        {
            ElementDisplay display = scene.Instantiate<ElementDisplay>();
            this.AddChild(display);
        }
        GameScene.activeEntities.Add(this);
    }

    private void handleMovement(double delta){
        // Convert velocity to motion
        Vector2 motion = velocity * (float)delta;
        
        // Handle collision
        KinematicCollision2D? collision = MoveAndCollide(motion);
        if (collision != null)
        {
            Vector2 normal = collision.GetNormal();
            if (Mathf.Abs(normal.X) > 0.1f)
                velocity.X = 0;
            if (Mathf.Abs(normal.Y) > 0.1f)
                velocity.Y = 0;
        }
        
        // Apply friction force
        if (velocity.Length() > 0.1f)  // Only apply friction if moving
        {
            float frictionForce = frictionMultiplier * 1000f;  // Base friction force
            Vector2 frictionDirection = -velocity.Normalized();
            Vector2 frictionVector = frictionDirection * frictionForce * (float)delta;
            
            // Apply friction but don't reverse movement direction
            if (frictionVector.Length() > velocity.Length())
            {
                velocity = Vector2.Zero;  // Stop completely
            }
            else
            {
                velocity += frictionVector;
            }
        }
        else
        {
            velocity = Vector2.Zero;  // Stop completely if very slow
        }
    }
    public override void _Process(double delta)
    {
        if (!isStatic){
            handleMovement(delta);
        }
        
        // Rest of the original code
        foreach (Effect effect in effects.ToArray())
        {
            effect.Update(delta);
            if (effect.duration <= 0)
            {
                effects.Remove(effect);
                effect.OnRemove();
            }
        }
        reactor.Update(delta);
    }

    public override void _ExitTree()
    {
        GameScene.activeEntities.Remove(this);
    }

    public virtual void OnHit(Damage damage){
        health -= damage.amount * nextDamageMultiplier;
        if (nextDamageMultiplier != 1){
            nextDamageMultiplier = 1;
        }
        // GD.Print(GetType().Name + " hit for " + damage + " pts. Remaining health: " + health);
        GameScene.ShowDamage(damage.amount, GlobalPosition, damage.element);
        
        if (damage.element != null && damage.elementAmount != null){
            reactor.AddElement(damage.element.Value, damage.elementAmount.Value);
        }
        
        if (damage.knockback != null){
            velocity += (Vector2)damage.knockback;
        }
        if (health <= 0) Die();
    }

    public void ApplyMotion(Vector2 motion){
        velocity += motion / mass;
    }

    public void OnHit(float damage, Vector2? knockback = null, Entity? source = null, Chemistry.Element? element = null, float? elementAmount = null)
    {
        OnHit(new Damage{amount = damage, knockback = knockback, source = source, element = element, elementAmount = elementAmount});
    }

    public void OnMeleeHit(float damage, Entity source, int knockbackImpulseValue = 0, Chemistry.Element? element = null){
        OnHit(damage, (this.GlobalPosition - source.GlobalPosition).Normalized() * knockbackImpulseValue, source, element);
    }
    public virtual void Die()
    {
        // 这里可以添加死亡动画、掉落物品等逻辑		
        if (deathEffectScene != null)
        {
            Node2D deathEffect = deathEffectScene.Instantiate<Node2D>();
            GetTree().Root.AddChild(deathEffect);
            deathEffect.GlobalPosition = GlobalPosition;
        }
        QueueFree();
    }

    public void onOverloaded(float elementAmount){
        // Explosion
        GameScene.ShowReaction(Reaction.Overloaded, this.GlobalPosition);
        GameScene.CreateExplosion(this.GlobalPosition, 10);
    }

    public void onElectroCharged(float elementAmount){
        // Static damage, small range AOE, give electro
        GameScene.ShowReaction(Reaction.ElectroCharged, this.GlobalPosition);
        GameScene.CreateAOE_Trigger(this.GlobalPosition, 60, (Entity entity) => {
            if (entity != this){
                entity.OnHit(5, element: Chemistry.Element.Electro, elementAmount: 10);
            }
        });
    }

    public void onSuperconduct(float elementAmount){
        // placeholder: damage multiply ?

    }

    public void onBurning(float elementAmount){
        // Empty implementation
        effects.Add(new BurningEffect(this, elementAmount * 10));
        GameScene.ShowReaction(Reaction.Burning, this.GlobalPosition);
    }

    public void onVaporize(float elementAmount){
        // 移除所有燃烧效果
        ClearEffect<BurningEffect>();
        
        // 显示蒸发反应
        GameScene.ShowReaction(Reaction.Vaporize, this.GlobalPosition);
        
        // 增加伤害倍率
        nextDamageMultiplier = 2f;
    }

    public void onMelt(float elementAmount){
        // static damage, give hydro?
        GameScene.ShowReaction(Reaction.Melt, this.GlobalPosition);
    }

    public void onFreeze(float elementAmount){
        // freeze
        GameScene.ShowReaction(Reaction.Freeze, this.GlobalPosition);
    }

    public void ClearEffect<T>() where T : Effect
    {
        effects.ForEach(effect => {
            if (effect is T){
                effect.OnRemove();
            }
        });
        effects.RemoveAll(effect => effect is T);
    }

}
