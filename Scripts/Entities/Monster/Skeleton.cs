using Godot;
using System;

public partial class Skeleton : LivingEntity
{
	[Export]
	private PackedScene bulletScene;
	private float ATTACK_INTERVAL = 1; //s
	private float attackTimer = 0.0f;
	private String lastPlayedAnimationName = "";
	private bool attackAnimationFinished = false;
	public override void _Ready()
	{
		base._Ready();
		animatedSprite2D.Modulate = Colors.White;
		group = Group.Enemy;
		animatedSprite2D.AnimationFinished += () =>
		{
			attackAnimationFinished = true;
		};
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
					randomDirection = new Vector2(
						(float)GD.RandRange(-1, 1),
						(float)GD.RandRange(-1, 1)
					).Normalized();
					timer = 1;
				}
				break;
			case State.Moving:
				playAnimation("moving");
				Velocity = randomDirection * 50;
				MoveAndSlide();

				// 添加转向逻辑
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
					Velocity = Vector2.Zero;
					state = State.Idle;
					randomDirection = new Vector2(
						(float)GD.RandRange(-1, 1),
						(float)GD.RandRange(-1, 1)
					).Normalized();
					timer = 1;
				}
				break;
			case State.Attack:
				if (attackAnimationFinished)
				{
					Bullet bullet = bulletScene.Instantiate<Bullet>();
					Node2D player = GameScene.player;
					Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
					bullet.GlobalPosition = GlobalPosition;
					bullet.velocity = direction * 100;
					bullet.caster = this;
					GameScene.instance.AddChild(bullet);
					state = State.Moving;
					randomDirection = new Vector2(
						GD.RandRange(-1, 1),
						GD.RandRange(-1, 1)
					).Normalized();
					timer = 1;
				}
				break;
		}
		attackTimer -= (float)delta;
		if (attackTimer <= 0)
		{
			Attack();
			attackTimer = ATTACK_INTERVAL; // 重置攻击计时器
		}
		base._Process(delta);
	}
	public override void Attack()
	{
		playAnimation("attack");
		attackAnimationFinished = false;
		state = State.Attack;
	}
	public override void OnHit(int damage)
	{
		base.OnHit(damage);
	}
	public override void Die()
	{
		base.Die();
	}
	public override void ApplyDamage(long amout = 0L, Vector2? direction = null, Entity source = null)
	{
		throw new NotImplementedException();
	}
	private void playAnimation(String animationName)
	{
		lastPlayedAnimationName = animationName;
		animatedSprite2D.Play(animationName);
	}
}
