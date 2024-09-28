using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class TipManager : Control
{
	// Called when the node enters the scene tree for the first time.
	
	[Export]
	public PackedScene tipLabelScene;

	[Export]
	public PackedScene damageLabelScene;

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

	public static void ShowDamage(float damage, Vector2 worldPosition)
	{
		Label damageLabel = instance.damageLabelScene.Instantiate<Label>();
		damageLabel.Text = damage.ToString();
		damageLabel.SetPosition(new Vector2(0,0),false);
		GD.Print("Global Position: " + damageLabel.GlobalPosition + " World Position: " + worldPosition);
		instance.AddChild(damageLabel);
	}
}
