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

	public ParamSourceDisplay paramSourceDisplay;
	private static SpellEditorBox editingBox = null;

	public override void _Ready()
	{
		spellEditor = this.GetParent<SpellEditor>();
		this.Connect("pressed", new Callable(this, "onItemClicked"));
		spellPieceIcon = GetNode<SpellPieceIcon>("SpellPieceIcon");
		spellPicker = spellEditor.getSpellPicker();
		spellPicker.Connect("item_clicked", new Callable(this, "onSelectionDone"));
		paramSourceDisplay = GetNode<ParamSourceDisplay>("ParamSourceDisplay");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void onItemClicked(){
		if (spellEditor.lastMouseButton == SpellEditor.LastMouseButton.Left){
			spellEditor.setSpellPickerAt(this);
			editingBox = this;
		}else if (spellEditor.lastMouseButton == SpellEditor.LastMouseButton.Right){
			spellEditor.setSpellPieceConfigPanelAt(this);
		}else if (spellEditor.lastMouseButton == SpellEditor.LastMouseButton.Middle){
			clear();
		}
	}

	public void onSelectionDone(int index, Vector2 atPosition, int mouseButtonIndex){
		if (editingBox == this){
			spellPicker.resetPosition();
			editingBox = null;
			selectedSpellPieceName = spellPicker.getSpellPieceName(index);
			spellPiece = spellPicker.getSpellPiece(index);
			GD.Print("SpellEditorBox selection done:" + selectedSpellPieceName);
			spellPieceIcon.setIconForSpellPiece(selectedSpellPieceName);
		}
	}

	public void clear(){
		spellPiece = null;
		SpellPieceParamDirection = new DPad.Direction[4];
		spellPieceIcon.clear();
	}





}
