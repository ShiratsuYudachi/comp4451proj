using Godot;
using System;

public partial class Tree : MapEntity
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		group = Group.Map;
		base._Ready();
		this.animatedSprite2D.Modulate = Colors.White;
		this.hardness = 20;
		this.MAX_HEALTH = 100;
		health = MAX_HEALTH;
		this.animatedSprite2D.Play("default");
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}
	public override void OnHit(Damage damage)
	{
		base.OnHit(damage);
	}
	
}
