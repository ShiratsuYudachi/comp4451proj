using Godot;
using System;

public partial class FreeAfterAnimation : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		AnimationPlayer animationPlayer = GetNode<AnimationPlayer>("Sprite2D/AnimationPlayer");
		animationPlayer.AnimationFinished += (animationName) =>
		{
			QueueFree();
		};
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
