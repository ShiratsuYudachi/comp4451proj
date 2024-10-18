#nullable enable
using Godot;
using System;
public abstract partial class LivingEntity : Entity
{
	protected enum State { Idle, Moving }
	protected float timer = 3;
	protected State state = State.Idle;
	protected Vector2 randomDirection;
	protected bool isRight = true;
	public Vector2 speed;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 offset = new Vector2((float)(speed.X * delta), (float)(speed.Y * delta));
		Position += offset;
		base._Process(delta);
	}
	public abstract void Attack();
	public override void OnHit(int damage)
	{
		base.OnHit(damage);
	}
	public override void Die()
	{
		base.Die();
	}
}
