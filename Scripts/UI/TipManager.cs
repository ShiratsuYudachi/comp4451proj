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


	public override void _Ready()
	{
		
	}

	public void ShowTip(string tip)
	{
		Label tipLabel = tipLabelScene.Instantiate<Label>();
		tipLabel.Text = tip;
		AddChild(tipLabel);
	}

	public  void ShowDamage(float damage, Vector2 worldPosition)
	{
		Label damageLabel = damageLabelScene.Instantiate<Label>();
		damageLabel.Text = damage.ToString();
		damageLabel.SetPosition(new Vector2(0,0),false);
		GD.Print("Global Position: " + damageLabel.GlobalPosition + " World Position: " + worldPosition);
		AddChild(damageLabel);
	}
}
