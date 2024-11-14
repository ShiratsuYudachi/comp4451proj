using Godot;
using System;
using System.Collections.ObjectModel;
public partial class SpellPicker : ItemList
{
	// Called when the node enters the scene tree for the first time.

	public Vector2 spellListInitialPosition;

	public override void _Ready()
	{
		spellListInitialPosition = this.Position;

		
		ReadOnlyCollection<SpellPieceInfo> spellPieces = SpellRegistry.GetAllSpellPieces();

		foreach (SpellPieceInfo spellPieceInfo in spellPieces)
		{
			GD.Print("Adding spell piece: " + spellPieceInfo.name);
			AddItem(spellPieceInfo.name);
		}
		// 设置所有项目为不可选择
		for (int i = 0; i < GetItemCount(); i++)
		{
			SetItemSelectable(i, false);
		}

	}

	public void resetPosition()
	{
		this.Position = spellListInitialPosition;
	}

	public string getSpellPieceName(int index)
	{
		return GetItemText(index);
	}
	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

	}

	public SpellPiece getSpellPiece(int index)
	{
		SpellPieceInfo spellPieceInfo = SpellRegistry.GetSpellPieceInfo(index);
		return (SpellPiece)spellPieceInfo.spellClassType.GetConstructor(Type.EmptyTypes).Invoke(null);
	}
}
