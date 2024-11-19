using Godot;
using System;
public partial class Necromancer : LivingEntity
{
    [Export]
    private PackedScene bulletScene;
    private PackedScene skeletonScene = (PackedScene)ResourceLoader.Load("res://Scenes/LivingEntities/Skeleton.tscn");
    public float ATTACK_INTERVAL = 5; // s
    private float attackTimer = 0.0f;
    private const int NUM_SUMMONED_SKELETONS_MAX = 3;
    private int summonedSkeletonCounter = 0;
    private String lastPlayedAnimationName = "";
    private bool attackAnimationFinished = false;
    public override void _Ready()
    {
        base._Ready();
        animatedSprite2D.Modulate = Colors.DimGray;
        group = Group.Enemy;
        animatedSprite2D.AnimationFinished += () => { attackAnimationFinished = true; };
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
                    timer = 2;
                    randomDirection = new Vector2(
                        GD.RandRange(-1, 1),
                        GD.RandRange(-1, 1)
                    ).Normalized();
                }
                break;
            case State.Moving:
                playAnimation("moving");
                Velocity = randomDirection * 25;
                MoveAndSlide();
                if (randomDirection.X > 0)
                {
                    isRight = true;
                }
                else if (randomDirection.X < 0)
                {
                    isRight = false;
                }
                animatedSprite2D.FlipH = !isRight;
                timer -= (float)delta;
                if (timer <= 0)
                {
                    state = State.Idle;
                    timer = 2;
                    randomDirection = new Vector2(
                        GD.RandRange(-1, 1),
                        GD.RandRange(-1, 1)
                    ).Normalized();
                }
                break;
            case State.Attack:
                if (attackAnimationFinished)
                {
                    Bullet bullet = bulletScene.Instantiate<Bullet>();
                    bullet.caster = this;
                    bullet.GlobalPosition = GlobalPosition;
                    Node2D player = GameScene.player;
                    Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
                    bullet.velocity = direction * 100;
                    GameScene.instance.AddChild(bullet);
                    if (summonedSkeletonCounter < NUM_SUMMONED_SKELETONS_MAX)
                    {
                        Node2D skeletonInstance = skeletonScene.Instantiate<Node2D>();
                        skeletonInstance.GlobalPosition = GlobalPosition;
                        GetParent().AddChild(skeletonInstance);
                        ++summonedSkeletonCounter;
                    }
                    state = State.Moving;
                    timer = 2;
                    randomDirection = new Vector2(
                        GD.RandRange(-1, 1),
                        GD.RandRange(-1, 1)
                    ).Normalized();
                }
                break;
            default:
                break;
        }
        attackTimer -= (float)delta;
        if (attackTimer <= 0)
        {
            Attack();
            attackTimer = ATTACK_INTERVAL;
        }
        base._Process(delta);
        if (hitFlashTimer <= 0)
        {
            animatedSprite2D.Modulate = Colors.DimGray;
        }
    }
    public override void Attack()
    {
        playAnimation("attack");
        attackAnimationFinished = false;
        state = State.Attack;
    }
    public override void Die()
    {
        base.Die();
    }
    private void playAnimation(String animationName)
    {
        lastPlayedAnimationName = animationName;
        animatedSprite2D.Play(animationName);
    }
}