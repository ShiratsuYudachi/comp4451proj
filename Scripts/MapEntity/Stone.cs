using Godot;
using System;

public partial class Stone : MapEntity
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{

		base._Ready();
		this.animatedSprite2D.Modulate = Colors.White;
		this.hardness = 10;
		this.animatedSprite2D.Play("default");
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
