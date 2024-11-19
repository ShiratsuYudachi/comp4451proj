using Godot;
using System;
using System.Numerics;
public partial class Slime : LivingEntity
{
    enum Type { Mother, Baby };
    Type type = Type.Mother;
    private String lastPlayedAnimationName = "";
    public override void _Ready()
    {
        base._Ready();
        group = Group.Enemy;
        if (type == Type.Mother)
        {
            MAX_HEALTH = 300;
            health = MAX_HEALTH;
        }
        else
        {
            Scale = new Godot.Vector2(0.5f, 0.5f);
            MAX_HEALTH = 100;
            health = MAX_HEALTH;
        }
        GetNode<Area2D>("HitBox").Connect("area_entered", new Callable(this, nameof(OnHitting)));
    }
    public override void _Process(double delta)
    {
        switch (state)
        {
            case State.Idle:
                playAnimation("idle");
                timer -= (float)delta;
                if (timer <= 0)
                {
                    state = State.Moving;
                    timer = 1;
                    randomDirection = (GameScene.player.GlobalPosition - GlobalPosition).Normalized();
                    Velocity = randomDirection * 50;
                }
                break;
            case State.Moving:
                playAnimation("moving");
                bool collided = MoveAndSlide();
                if (randomDirection.X > 0)
                {
                    isRight = true;
                }
                else if (randomDirection.Y < 0)
                {
                    isRight = false;
                }
                animatedSprite2D.FlipH = !isRight;
                timer -= (float)delta;
                if (timer <= 0)
                {
                    state = State.Idle;
                    timer = 1;
                }
                break;
            default:
                break;
        }
        base._Process(delta);
    }
    public override void Attack()
    {
        throw new NotImplementedException("Slime shouldn't call Attack method!");
    }
    public override void Die()
    {
        if (type == Type.Mother)
        {
            CallDeferred(nameof(split));
        }
        base.Die();
    }
    private void split()
    {
        for (int i = 0; i < 3; ++i)
        {
            PackedScene slimeScene = (PackedScene)ResourceLoader.Load("res://Scenes/LivingEntities/Slime.tscn");
            Node2D slimeInstance = slimeScene.Instantiate<Node2D>();
            ((Slime)slimeInstance).type = Type.Baby;
            slimeInstance.GlobalPosition = GlobalPosition + new Godot.Vector2(GD.RandRange(-10, 10), GD.RandRange(-10, 10));
            GetParent().AddChild(slimeInstance);
        }
    }
    private void playAnimation(String animationName)
    {
        lastPlayedAnimationName = animationName;
        animatedSprite2D.Play(animationName);
    }
    public void OnHitting(Area2D area)
    {
        if (!area.IsInGroup("HitBox")) return;
        Node nodeOnHit = area.GetParent();
        if (nodeOnHit is LivingEntity livingEntityOnHit)
        {
            if (livingEntityOnHit.group == group) return;
            if (type == Type.Mother)
            {
                livingEntityOnHit.OnMeleeHit(10, this, 100);
            }
            else
            {
                livingEntityOnHit.OnMeleeHit(5, this, 50);
            }
        }
        else if (nodeOnHit is MapEntity mapEntityOnHit)
        {
            if (type == Type.Mother)
            {
                mapEntityOnHit.OnHit(10);
            }
            else
            {
                mapEntityOnHit.OnHit(5);
            }
        }
        //Velocity = Godot.Vector2.Zero;
        if (state == State.Moving) GlobalPosition -= randomDirection * 5;
        state = State.Idle;
        timer = 1;
    }
}