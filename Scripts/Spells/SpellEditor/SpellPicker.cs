using Godot;
using System;

public partial class SpellPicker : ItemList
{
	// Called when the node enters the scene tree for the first time.

	public Vector2 spellListInitialPosition;

	public override void _Ready()
	{
		spellListInitialPosition = this.Position;
		Texture2D texture = GD.Load<Texture2D>("res://2dres/shot.png");
		string[] spellPiecesList = new string[] {
			"Blink",
			"GetEntityPos",
			"VectorMinus"
		};

		foreach (string spellPiece in spellPiecesList)
		{
			AddItem(spellPiece, texture);
		}
		// 设置所有项目为不可选择
		for (int i = 0; i < GetItemCount(); i++)
		{
			SetItemSelectable(i, false);
		}
		
	}

	public void resetPosition(){
		this.Position = spellListInitialPosition;
	}

	public string getSpellPieceName(int index){
		return GetItemText(index);
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}
}
