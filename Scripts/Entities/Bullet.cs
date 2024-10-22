using Godot;
using System;

public partial class Bullet : Node2D
{
	public Vector2 velocity;
	public float timer = 5;
	public Entity caster;
	// Called when the node enters the scene tree for the first time.

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
		if (area.Name == "MonsterDetector") return;
		Node nodeOnHit = area.GetParent();

		if (nodeOnHit is Entity entityOnHit && entityOnHit.group == caster.group) return;
		
		if (nodeOnHit is LivingEntity livingEntity && livingEntity != caster)
		{
			livingEntity.OnHit(50);
			QueueFree();
		}
		else if (nodeOnHit is MapEntity mapEntity)
		{
			mapEntity.OnHit(5);
			QueueFree();
		}
	}
}
