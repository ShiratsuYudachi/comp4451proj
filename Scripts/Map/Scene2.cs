using Godot;
using System;

public partial class Scene2 : GameScene
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		base._Ready();
		AddChild(ScenePortal.player);
		AddChild(ScenePortal.ui);
		ScenePortal.player.Position = ScenePortal.playerPos;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
	}
}
