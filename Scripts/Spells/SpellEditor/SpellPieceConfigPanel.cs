using Godot;
using System;

public partial class SpellPieceConfigPanel : Control
{
	[Export]
	public PackedScene paramSelector;

	public Vector2 spellPieceConfigPanelInitialPosition;

	public Button doneButton;

	public SpellEditorBox editorBoxAttached;
	public ParamSelector[] paramSelectors;
	public override void _Ready()
	{
		spellPieceConfigPanelInitialPosition = this.Position;
		doneButton = GetNode<Button>("DoneButton");
		doneButton.Connect("button_down", new Callable(this, "doneButtonPressed"));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void resetPosition(){
		this.Position = spellPieceConfigPanelInitialPosition;
	}


	public void setupParamSelectorsAt(SpellEditorBox spellEditorBox){
		editorBoxAttached = spellEditorBox;
		paramSelectors = new ParamSelector[spellEditorBox.spellPiece.ParamList.Length];

		for (int i = 0; i < spellEditorBox.spellPiece.ParamList.Length; i++){
			SpellVariableType type = spellEditorBox.spellPiece.ParamList[i];
			ParamSelector newParamSelector = paramSelector.Instantiate<ParamSelector>();

			newParamSelector.setParamName(type.ToString());
			this.GetNode<Control>("VAlign").AddChild(newParamSelector);
			paramSelectors[i] = newParamSelector;
		}
		// load parameter
		for (int i = 0; i < spellEditorBox.spellPiece.ParamList.Length; i++){
			paramSelectors[i].dPad.SetDirection(spellEditorBox.SpellPieceParamDirection[i]);
		}
	}

	public void doneButtonPressed(){
		writeParamDirectionToSpellPiece();
		QueueFree();
	}

	public void writeParamDirectionToSpellPiece(){
		for (int i = 0; i < editorBoxAttached.spellPiece.ParamList.Length; i++){
			editorBoxAttached.SpellPieceParamDirection[i] = paramSelectors[i].dPad.GetDirection();
		}
	}
}
