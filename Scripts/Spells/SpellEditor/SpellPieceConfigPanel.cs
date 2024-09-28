using Godot;
using System;

public partial class SpellPieceConfigPanel : Control
{
	
	public Vector2 spellPieceConfigPanelInitialPosition;
	public override void _Ready()
	{
		spellPieceConfigPanelInitialPosition = this.Position;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void resetPosition(){
		this.Position = spellPieceConfigPanelInitialPosition;
	}
}
