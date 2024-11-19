using Godot;
using System;
using System.Collections.Generic;

public partial class DungeonScene : GameScene
{
	
	int waveNumber = 0;

	float timeElapsed = 0;
	
	public bool levelCleared = false;
	

	List<ScenePortal> portals = new List<ScenePortal>();

	public override void _Ready()
	{
		foreach (Node2D portal in GetTree().GetNodesInGroup("ScenePortal")){
			portals.Add(portal as ScenePortal);
			portal.Hide();
		}
		base._Ready();
	}

	public void showPortals(){
		foreach (ScenePortal portal in portals){
			portal.Show();
		}
	}


	public virtual void StartWave(int _waveNumber){
		switch (_waveNumber){
			case 1:
				GameScene.SpawnEntity("Skeleton", new Vector2(-50, 0));
				GameScene.SpawnEntity("Skeleton", new Vector2(0, 50));
				GameScene.SpawnEntity("Skeleton", new Vector2(50, 0));
				GameScene.SpawnEntity("Skeleton", new Vector2(0, -50));
				GD.Print("[INFO] DungeonScene: Started wave 1");
				break;
			case 2:
				GameScene.SpawnEntity("Zombie", new Vector2(-50, 0));
				GameScene.SpawnEntity("Zombie", new Vector2(0, 50));
				GameScene.SpawnEntity("Zombie", new Vector2(50, 0));
				GameScene.SpawnEntity("Zombie", new Vector2(0, -50));
				GD.Print("[INFO] DungeonScene: Started wave 2");
				break;
			default:
				levelCleared = true;
				GD.Print("[INFO] DungeonScene: Level cleared");
				showPortals();
				break;
		}
	}

	public bool checkWaveCompleted(){
		var enemies = GetTree().GetNodesInGroup("Enemy");
		if (enemies.Count == 0)
		{
			GD.Print("[INFO] DungeonScene: Wave completed");
			return true;
		}
		return false;
	}

	public void enablePortal(){

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		base._Process(delta);
		if (levelCleared){
			return;
		}
		if (checkWaveCompleted()){
			StartWave(++waveNumber);
		}

		

		timeElapsed += (float)delta;
		
	}
}
