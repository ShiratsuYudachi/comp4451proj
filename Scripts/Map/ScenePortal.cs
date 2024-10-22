using Godot;
using System;

public partial class ScenePortal : Area2D
{
	[Export]
	public PackedScene targetScene;

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
		if (area.GetParent() is PlayerControl)
		{
			isTeleporting = true;
			playerPos = new Vector2(0, 0);
			player = area.GetParent<PlayerControl>();
			ui = GetTree().Root.GetNode("Scene1").GetNode<CanvasLayer>("CanvasLayer");
			GetTree().Root.GetNode("Scene1").CallDeferred("remove_child", player);
			GetTree().Root.GetNode("Scene1").CallDeferred("remove_child", ui);

			GetTree().CallDeferred("change_scene_to_packed", targetScene);
		}
	}
}
