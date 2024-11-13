#nullable enable
using Godot;
using System;

public partial class PlayerControl : LivingEntity
{
	public const float Speed = 100.0f;
	private AnimatedSprite2D? _animatedSprite2D;

	public SpellCaster? spellCaster;

	public override void _Ready()
	{
		base._Ready();
		_animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		spellCaster = GetNode<SpellCaster>("SpellCaster");
		health = 114514;
		group = Group.Player;
		effects.Add(new BurningEffect(this, 5));
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
		if (_animatedSprite2D != null)
		{
			_animatedSprite2D.FlipH = !isRight;
		}
		else
		{
			GD.PushError("No AnimatedSprite2D node found!");
			GetTree().Quit();
		}

		if (inputVector.X != 0 || inputVector.Y != 0)
		{
			if (_animatedSprite2D != null)
			{
				_animatedSprite2D.Play("move");
			}
			else
			{
				GD.PushError("No AnimatedSprite2D node found!");
				GetTree().Quit();
			}
		}
		else
		{
			if (_animatedSprite2D != null)
			{
				_animatedSprite2D.Play("idle");
			}
		}

		if (Input.IsActionJustPressed("Attack"))
		{
			spellCaster?.Cast(0);
		}
		if (Input.IsActionJustPressed("Cast"))
		{
			spellCaster?.Cast(1);
		}

		if (Input.IsActionJustPressed("Key1"))
		{
			spellCaster?.Cast(2);
		}	
		if (Input.IsActionJustPressed("Key2"))
		{
			spellCaster?.Cast(3);
		}
		if (Input.IsActionJustPressed("Key3"))
		{
			spellCaster?.Cast(4);
		}
		if (Input.IsActionJustPressed("Key4"))
		{
			spellCaster?.Cast(5);
		}


		
		

	}
	public override void Attack()
	{
	}
	public override void ApplyDamage(long amount = 0L, Vector2? direction = null, Entity? source = null)
	{

	}
}
