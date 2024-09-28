using Godot;
using System;

public partial class PlayerControl : LivingEntity
{
	public const float Speed = 100.0f;
	private AnimatedSprite2D _animatedSprite2D;
	private bool isRight = true;

	public override void _Ready()
	{
		base._Ready();
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		health = 114514;
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 inputVector = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
		Velocity = inputVector * Speed;
		MoveAndSlide();

		if (inputVector.X > 0) 
		{
			isRight = true;
			EventManager.PlayerChangeDirection?.Invoke(true);
		}
		else if (inputVector.X < 0)
		{
			isRight = false;
			EventManager.PlayerChangeDirection?.Invoke(false);
		}
		_animatedSprite2D.FlipH = !isRight;

		if (inputVector.X != 0 || inputVector.Y != 0)
		{
			_animatedSprite2D.Play("move");
		}
		else
		{
			_animatedSprite2D.Play("idle");
		}
	}
}
