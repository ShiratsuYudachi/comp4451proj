using Godot;
using System;

public partial class Bullet : Node2D, IMassEntity
{
	// Configurable
	public Vector2 velocity { get; set; }
	public int mass { get; set; } = 1;
	public float timer = 5;

	// Internal
	public Vector2 massPosition
	{
		get => GlobalPosition;
		set => GlobalPosition = value;
	}
	public Vector2 massVelocity
	{
		get => velocity;
		set => velocity = value;
	}

	public Entity caster;
	// Called when the node enters the scene tree for the first time.

	public override void _Ready()
	{
		GetNode<Area2D>("HitBox").Connect("area_entered", new Callable(this, nameof(OnHit)));
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
		if (!area.IsInGroup("HitBox")) return;
		Node nodeOnHit = area.GetParent();

		if (nodeOnHit is Entity entityOnHit && entityOnHit.group == caster.group) return;

		if (nodeOnHit is LivingEntity livingEntityOnHit && livingEntityOnHit != caster)
		{
			livingEntityOnHit.OnHit(10);
			QueueFree();
		}
		else if (nodeOnHit is MapEntity mapEntityOnHit)
		{
			mapEntityOnHit.OnHit(10);
			QueueFree();
		}
	}
}
