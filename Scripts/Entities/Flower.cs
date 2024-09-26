using Godot;
using System;

public partial class Flower : AnimatedSprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Area2D collisionArea = GetNode<Area2D>("Area2D");
		collisionArea.Connect("body_entered", new Callable(this, "OnBodyEntered"));
		collisionArea.Connect("body_exited", new Callable(this, "OnBodyExited"));
	}

	public void OnBodyEntered(Node2D body)
	{
		this.Play("onStepped");
	}

	public void OnBodyExited(Node2D body)
	{
		this.Play("default");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
