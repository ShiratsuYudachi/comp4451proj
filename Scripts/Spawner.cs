using Godot;
using System;

public partial class Spawner : Node2D
{
	[Export]
	public PackedScene entityScene;
	[Export]
	public int entityNumLimit = 5;

	private float SPAWN_INTERVAL = 2f;

	private float spawnCooldown = 0f;

	// All entities generated by the spawner will be added as a child 
	private int getCurrentEntityNum(){
		return this.GetChildren().Count;
	}
	
	public override void _Process(double delta)
	{
		if (getCurrentEntityNum() < entityNumLimit)
		{
			spawnCooldown -= (float)delta;
			if (spawnCooldown <= 0)
			{
				Node2D entity = entityScene.Instantiate<Node2D>();
				this.AddChild(entity);
				spawnCooldown = SPAWN_INTERVAL;
			}
		}
	}

}
