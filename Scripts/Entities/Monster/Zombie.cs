using Godot;
using System;

public partial class Zombie : LivingEntity
{
	private String lastPlayedAnimationName = "";
	public override void _Ready()
	{
		base._Ready();
		group = Group.Enemy;
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
					randomDirection = new Vector2(GD.RandRange(-1, 1), GD.RandRange(-1, 1)).Normalized();
					Velocity = randomDirection * 25;
				}
				break;
			case State.Moving:
				playAnimation("moving");
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
					timer = 3;
				}
				break;
			default:
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