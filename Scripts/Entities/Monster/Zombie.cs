using Godot;
using System;

public partial class Zombie : LivingEntity
{
	public override void _Ready()
	{
		base._Ready();
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		switch (state)
		{
			case State.Idle:
				animatedSprite2D.Play("idle");
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
				animatedSprite2D.Play("moving");
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
					timer = 3;
					randomDirection = new Vector2(
						(float)GD.RandRange(-1, 1),
						(float)GD.RandRange(-1, 1)
					).Normalized();
					timer = 1;
				}
				break;
		}
		base._Process(delta);
	}
	public override void Attack()
	{
	}
	public override void OnHit(int damage)
	{

		base.OnHit(damage);
	}

	public override void Die()
	{
		base.Die();
	}
	public override void ApplyDamage(long amout = 0, Vector2? direction = null, Entity source = null)
	{

	}
}
