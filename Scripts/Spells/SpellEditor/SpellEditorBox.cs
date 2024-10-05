using Godot;
using System;

public partial class SpellEditorBox : Button
{
	// Called when the node enters the scene tree for the first time.

	public SpellPicker spellPicker;
	public GridContainer editorGrid;
	public SpellEditor spellEditor;
	public string selectedSpellPieceName = "";

	public SpellPieceIcon spellPieceIcon;


	public SpellPiece spellPiece;

	public DPad.Direction[] SpellPieceParamDirection = new DPad.Direction[4];


	private bool isEditing = false;

	public override void _Ready()
	{
		spellEditor = this.GetParent<SpellEditor>();
		this.Connect("pressed", new Callable(this, "onItemClicked"));
		spellPieceIcon = GetNode<SpellPieceIcon>("SpellPieceIcon");
		spellPicker = spellEditor.getSpellPicker();
		spellPicker.Connect("item_clicked", new Callable(this, "onSelectionDone"));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void onItemClicked(){
		if (spellEditor.lastMouseButton == SpellEditor.LastMouseButton.Left){
			spellEditor.setSpellPickerAt(this);
			isEditing = true;
		}else if (spellEditor.lastMouseButton == SpellEditor.LastMouseButton.Right){
			spellEditor.setSpellPieceConfigPanelAt(this);
		}
	}

	public void onSelectionDone(int index, Vector2 atPosition, int mouseButtonIndex){
		if (isEditing){
			spellPicker.resetPosition();
			isEditing = false;
			selectedSpellPieceName = spellPicker.getSpellPieceName(index);
			spellPiece = spellPicker.getSpellPiece(index);
			GD.Print("SpellEditorBox selection done:" + selectedSpellPieceName);
			spellPieceIcon.setIconForSpellPiece(selectedSpellPieceName);
		}
	}





}
