using Godot;
using System;

public partial class SpellPieceIcon : TextureRect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void setIconForSpellPiece(string name){
		Texture2D texture = GD.Load<Texture2D>("res://Images/spells/"+name+".png");
		this.Texture = texture;
	}
}
