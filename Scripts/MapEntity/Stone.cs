using Godot;
using System;

public partial class Stone : MapEntity
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MAX_HEALTH = 500;
		health = MAX_HEALTH;
		group = Group.Map;
		base._Ready();
		animatedSprite2D.Modulate = Colors.White;
		hardness = 10;
		animatedSprite2D.Play("default");
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}

	public override void ApplyDamage(long amout = 0, Vector2? direction = null, Entity source = null)
	{
	}
}
