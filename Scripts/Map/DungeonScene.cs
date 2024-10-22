using Godot;
using System;

public partial class DungeonScene : GameScene
{
	
	int waveNumber = 0;

	float timeElapsed = 0;

	bool waveCompleted = true;
	

	public override void _Ready()
	{
		
		base._Ready();
	}

	public void StartWave(int _waveNumber){
		switch (_waveNumber){
			case 1:
				GameScene.SpawnEntity("Skeleton", new Vector2(-50, 0));
				GameScene.SpawnEntity("Skeleton", new Vector2(0, 50));
				GameScene.SpawnEntity("Skeleton", new Vector2(50, 0));
				GameScene.SpawnEntity("Skeleton", new Vector2(0, -50));
				GD.Print("[INFO] DungeonScene: Started wave 1");
				break;
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		if (timeElapsed > 1 && waveCompleted)
		{
			GD.Print("[INFO] DungeonScene: Starting wave 1");
			StartWave(++waveNumber);
			waveCompleted = false;
		}
		timeElapsed += (float)delta;
		base._Process(delta);
	}
}
