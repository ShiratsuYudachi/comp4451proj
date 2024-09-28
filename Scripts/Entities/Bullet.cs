using Godot;
using System;

public partial class Bullet : Node2D
{
	// Called when the node enters the scene tree for the first time.

	public Vector2 velocity;
	public float timer = 1;

	public Node caster;

	public override void _Ready()
	{
		GetNode<Area2D>("Area2D").Connect("area_entered", new Callable(this, nameof(OnHit)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Position += velocity * (float)delta;
		timer -= (float)delta;
		if (timer <= 0)
		{
			QueueFree();
		}
	}

	public void OnHit(Area2D area)
	{
		if (area.GetParent() is LivingEntity livingEntity && livingEntity != caster)
		{
			livingEntity.OnHit(10);
			QueueFree();
		}
	}
}
