using Godot;
using System;

public partial class TipManager : Control
{
	// Called when the node enters the scene tree for the first time.
	
	[Export]
	public PackedScene tipLabelScene;
	public static TipManager instance;

	public override void _Ready()
	{
		instance = this;
		ShowTip("Tip: Use WASD to move");
	}

	public static void ShowTip(string tip)
	{
		Label tipLabel = instance.tipLabelScene.Instantiate<Label>();
		tipLabel.Text = tip;
		instance.AddChild(tipLabel);
	}
}
