using Godot;
using System;

public partial class Zombie : LivingEntity
{
	public override void _Ready()
	{
		base._Ready();
		group = Group.Enemy;
		GetNode<Area2D>("HitBox").Connect("area_entered", new Callable(this, nameof(OnHitting)));
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
		throw new NotImplementedException("Zombie shouldn't call Attack()!");
	}

	public override void Die()
	{
		base.Die();
	}
	public void OnHitting(Area2D area)
	{
		if (!area.IsInGroup("HitBox")) return;
		Node nodeOnHit = area.GetParent();
		if (nodeOnHit is LivingEntity livingEntityOnHit)
		{
			if (livingEntityOnHit.group == group) return;
			livingEntityOnHit.OnMeleeHit(20, this, 200);
		}
		else if (nodeOnHit is MapEntity mapEntityOnHit)
		{
			mapEntityOnHit.OnHit(20);
		}
		if (state == State.Moving) GlobalPosition -= randomDirection * 5;
		state = State.Idle;
		timer = 1;
	}
}