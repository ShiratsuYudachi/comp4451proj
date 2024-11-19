using Godot;
using System;

public partial class Weapon1 : Sprite2D
{
	// Called when the node enters the scene tree for the first time.
	bool isRight = false;
	AnimationPlayer _animationPlayer;
	public override void _Ready()
	{
		this.Visible = false;
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
		if (Input.IsActionJustPressed("MeleeAttack") && !_animationPlayer.IsPlaying())
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
		if (!this.Visible) return;
		Node node = area.GetParent();
		if (node is LivingEntity livingEntity)
		{
			livingEntity.OnMeleeHit(10, this.GetParent().GetParent<LivingEntity>());
		}
		else if (node is MapEntity mapEntity)
		{
			mapEntity.OnMeleeHit(10, this.GetParent().GetParent<LivingEntity>());
		}
	}
}
