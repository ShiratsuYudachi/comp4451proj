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
	public Vector2 velocity;
	public float friction = 0.98f;

	protected float hitFlashDuration = 0.2f;
    protected float hitFlashTimer = 0f;
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 offset = new Vector2((float)(velocity.X * delta), (float)(velocity.Y * delta));
		Position += offset;
		velocity *= friction;

		if (hitFlashTimer > 0)
        {
            hitFlashTimer -= (float)delta;
            if (hitFlashTimer <= 0)
            {
                if (animatedSprite2D == null)
                {
                    GD.Print("No AnimatedSprite2D found!");
                    GetTree().Quit();
                }
                else
                {
                    animatedSprite2D.Modulate = Colors.White;
                }
            }
        }

		base._Process(delta);
	}
	public abstract void Attack();
	public override void OnHit(int damage)
	{
		if (animatedSprite2D == null)
        {
            GD.Print("Play OnHit animation failed! No AnimatedSprite2D found!");
            GetTree().Quit();
        }
        else
        {
            animatedSprite2D.Modulate = Colors.Red;
        }
        hitFlashTimer = hitFlashDuration;
		base.OnHit(damage);
	}
	public override void Die()
	{
		base.Die();
	}
}
