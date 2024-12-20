using Godot;
using System;

public partial class Flower : MapEntity
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		MAX_HEALTH = 10;
		health = MAX_HEALTH;
		group = Group.Map;
		base._Ready();
		animatedSprite2D.Modulate = Colors.White;
		hardness = 5;
		Area2D collisionArea = GetNode<Area2D>("HitBox");
		collisionArea.BodyEntered += OnBodyEntered;
		//collisionArea.Connect("body_entered", new Callable(this, "OnBodyEntered"));
		collisionArea.BodyExited += OnBodyExited;
		//collisionArea.Connect("body_exited", new Callable(this, "OnBodyExited"));
		animatedSprite2D.Play("default");
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}
	public void OnBodyEntered(Node2D body)
	{
		this.animatedSprite2D.Play("onStepped");
	}
	public void OnBodyExited(Node2D body)
	{
		this.animatedSprite2D.Play("default");
	}

	public override void Die()
	{
		base.Die();
	}

}
