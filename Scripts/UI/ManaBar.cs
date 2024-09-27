using Godot;
using System;

public partial class ManaBar : ProgressBar
{
	
	Label manaTextLabel;
	SpellCaster playerSpellCaster;
	public override void _Ready()
	{
		manaTextLabel = GetNode<Label>("ManaText");
		playerSpellCaster = GetTree().GetNodesInGroup("Player")[0].GetNode<SpellCaster>("SpellCaster");
		manaTextLabel.Text = "0";
	}

	
	public override void _Process(double delta)
	{
		manaTextLabel.Text = Mathf.Round(playerSpellCaster.Mana).ToString();
		Value = playerSpellCaster.Mana / playerSpellCaster.ManaMax * 100;
	}
}
