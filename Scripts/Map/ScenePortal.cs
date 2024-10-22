using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class ScenePortal : Area2D
{

	[Export]
	public String targetSceneName;

	public static PlayerControl player;
	public static CanvasLayer ui;
	public static Vector2 playerPos;

	public static bool isTeleporting = false;

	public override void _Ready()
	{
		this.Connect("area_entered", new Callable(this, nameof(onPlayerEnter)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void onPlayerEnter(Area2D area)
	{
		GD.Print("[INFO] ScenePortal: Player entered");
		if (area.GetParent() is PlayerControl && area.GetName() != "MonsterDetector")
		{			
			this.CallDeferred("_deferred_switch_scene", targetSceneName);
			
			//GetTree().CallDeferred("change_scene_to_packed", targetScene.ResourcePath);
		}
	}

	public static void _deferred_switch_scene(String targetSceneName){
		GD.Print("[INFO] ScenePortal: Switching scene to " + targetSceneName);
		Node2D sourceSceneNode = GameScene.instance;
		Window window = sourceSceneNode.GetTree().Root;
		GameScene newScene = GD.Load<PackedScene>("res://Scenes/GameScene/"+targetSceneName+".tscn").Instantiate<GameScene>();
		window.AddChild(newScene);
		GameScene.player.Reparent(newScene);
		GameScene.UI.Reparent(newScene);
		GameScene.instance = newScene;
		sourceSceneNode.Free();
		
	}
}
