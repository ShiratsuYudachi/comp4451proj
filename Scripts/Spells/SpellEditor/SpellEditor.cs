using Godot;
using System;

public partial class SpellEditor : GridContainer
{
	[Export]
	public SpellPicker spellPicker;

	[Export]
	public PackedScene spellPieceConfigPanelScene;

	public SpellPieceConfigPanel spellPieceConfigPanel;


	public enum LastMouseButton{
		Left,
		Right,
		Middle
	}
	public LastMouseButton lastMouseButton = LastMouseButton.Left;


	private int currentActivePanelIndex = -1;

	
	public override void _Ready()
	{
	}

	
	public override void _Process(double delta)
	{
		if (Input.GetMouseButtonMask() == MouseButtonMask.Left){
			lastMouseButton = LastMouseButton.Left;
		}
		else if (Input.GetMouseButtonMask() == MouseButtonMask.Right){
			lastMouseButton = LastMouseButton.Right;
		}

		else if (Input.IsActionJustPressed("Cancel"))
		{
			closeExistingPanel();
		}

		if (Input.IsActionJustPressed("Enter"))
		{
			CompileSpell().PrintTree();
		}
	}

	public SpellPicker getSpellPicker(){
		return spellPicker;
	}



	public void setSpellPieceConfigPanelAt(SpellEditorBox spellEditorBox){
		if (spellEditorBox.spellPiece == null) return;
		closeExistingPanel();
		spellPieceConfigPanel = spellPieceConfigPanelScene.Instantiate<SpellPieceConfigPanel>();
		this.GetParent().AddChild(spellPieceConfigPanel);
		spellPieceConfigPanel.Position = spellEditorBox.Position*this.Scale + this.Position;
		spellPieceConfigPanel.setupParamSelectorsAt(spellEditorBox);
	}

	public void setSpellPickerAt(SpellEditorBox spellEditorBox){
		closeExistingPanel();
		spellPicker.Position = spellEditorBox.Position*this.Scale + this.Position;
	}

	public void closeExistingPanel(){
		if (spellPieceConfigPanel != null && IsInstanceValid(spellPieceConfigPanel)){
			spellPieceConfigPanel.QueueFree();
		}
		spellPicker.resetPosition();
	}

	

	private Vector2 getBoxCoordinateInGrid(SpellEditorBox spellEditorBox){
		return new Vector2(spellEditorBox.GetIndex()%this.Columns, spellEditorBox.GetIndex()/this.Columns);
	}

	private SpellEditorBox getBoxAtCoordinate(Vector2 coordinate){
		foreach (SpellEditorBox spellEditorBox in this.GetChildren()){
			if (getBoxCoordinateInGrid(spellEditorBox) == coordinate) return spellEditorBox;
		}
		return null;
	}

	public SpellEvaluationTreeNode CompileSpell(){
		foreach (SpellEditorBox spellEditorBox in this.GetChildren()){
			if (spellEditorBox.spellPiece is ExecutorSpellPiece) return CompileBox(spellEditorBox);
		}
		return null;
	}

	public SpellEvaluationTreeNode CompileBox(SpellEditorBox spellEditorBox){
		if (spellEditorBox.spellPiece is SelectorSpellPiece) return new SpellEvaluationTreeNode(spellEditorBox.spellPiece);
		SpellEvaluationTreeNode root = new SpellEvaluationTreeNode(spellEditorBox.spellPiece);
		root.childrenSpellPieces = new SpellEvaluationTreeNode[root.rootSpellPiece.ParamList.Length];

		for (int i = 0; i < root.childrenSpellPieces.Length; i++){
			DPad.Direction paramDirection = spellEditorBox.SpellPieceParamDirection[i];
			Vector2 targetBoxCoordinateAtGrid = getBoxCoordinateInGrid(spellEditorBox);
			switch(paramDirection){
				case DPad.Direction.UP:
					targetBoxCoordinateAtGrid.Y -= 1;
					break;
				case DPad.Direction.DOWN:
					targetBoxCoordinateAtGrid.Y += 1;
					break;
				case DPad.Direction.LEFT:
					targetBoxCoordinateAtGrid.X -= 1;
					break;
				case DPad.Direction.RIGHT:
					targetBoxCoordinateAtGrid.X += 1;
					break;
			}
			SpellEditorBox targetBox = getBoxAtCoordinate(targetBoxCoordinateAtGrid);
			root.childrenSpellPieces[i] = CompileBox(targetBox);
		}

		return root;
	}


	// public TextureRect NewSpellPieceIcon(string name){
	// 	TextureRect icon = spellPieceIcon.Instantiate<TextureRect>();
	// 	icon.Texture = GD.Load<Texture2D>("res://images/spells/"+name+".png");
	// 	return icon;
	// }
}
