using Godot;
using System;
using System.Collections.ObjectModel;
public partial class SpellPicker : ItemList
{
	// Called when the node enters the scene tree for the first time.

	public Vector2 spellListInitialPosition;

	[Export]
	public Button executorButton;

	[Export]
	public Button operatorButton;

	[Export]
	public Button selectorButton;


	public override void _Ready()
	{
		spellListInitialPosition = this.Position;

		
		refreshItems(typeof(SpellPiece));

		executorButton.Pressed+=()=>{
			refreshItems(typeof(ExecutorSpellPiece));
		};

		operatorButton.Pressed+=()=>{
			refreshItems(typeof(OperatorSpellPiece));
		};

		selectorButton.Pressed+=()=>{
			refreshItems(typeof(SelectorSpellPiece));
		};

	}

	public void refreshItems(Type SpellPieceType){
		Clear();
		ReadOnlyCollection<SpellPieceInfo> spellPieces = SpellRegistry.GetAllSpellPieces();

		foreach (SpellPieceInfo spellPieceInfo in spellPieces)
		{
			if (spellPieceInfo.spellClassType.IsSubclassOf(SpellPieceType)){
				AddItem(spellPieceInfo.name);
			}
		}
		
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
		SpellPieceInfo spellPieceInfo = SpellRegistry.GetSpellPieceInfo(getSpellPieceName(index));
		return (SpellPiece)spellPieceInfo.spellClassType.GetConstructor(Type.EmptyTypes).Invoke(null);
	}
}
