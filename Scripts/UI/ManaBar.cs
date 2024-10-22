using Godot;
using System;

public partial class ManaBar : ProgressBar
{
	
	Label manaTextLabel;
	SpellCaster playerSpellCaster;
	public override void _Ready()
	{
		manaTextLabel = GetNode<Label>("ManaText");
		manaTextLabel.Text = "0";
	}

	
	public override void _Process(double delta)
	{
		if (playerSpellCaster == null)
		{
			playerSpellCaster = GameScene.player.GetNode<SpellCaster>("SpellCaster");
			return;
		}
		manaTextLabel.Text = Mathf.Round(playerSpellCaster.Mana).ToString();
		Value = playerSpellCaster.Mana / playerSpellCaster.ManaMax * 100;
	}
}
