#nullable enable
using Godot;
using System;
public abstract partial class LivingEntity : Entity
{
    protected enum State { Idle, Moving, Attack }
    protected float timer = 3;
    protected State state = State.Idle;
    protected Vector2 randomDirection;
    protected bool isRight = true;

    protected float hitFlashDuration = 0.2f;
    protected float hitFlashTimer = 0f;
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Ready()
    {
        base._Ready();
    }
    public override void _Process(double delta)
    {

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
	public override void OnHit(Damage damage)
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
