using Godot;
using System;

public partial class Weapon1 : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	bool isRight = false;
	AnimationPlayer _animationPlayer;
	public override void _Ready()
	{
		GetNode<Area2D>("Area2D").Connect("area_entered", new Callable(this, nameof(OnHitEnemy)));
		_animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");	
		EventManager.PlayerChangeDirection += (direction) =>
		{
			isRight = direction;
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!this.Visible)
		{
			return;
		}
		if (Input.IsActionJustPressed("Attack") && !_animationPlayer.IsPlaying())
		{
			if (isRight)
			{
				_animationPlayer.Play("attack_r");
			}
			else
			{
				_animationPlayer.Play("attack_l");
			}
		}
	}

	public void OnHitEnemy(Area2D area)
	{
		if (area.GetParent() is LivingEntity livingEntity)
		{
			livingEntity.OnHit(10);
		}
	}
}
