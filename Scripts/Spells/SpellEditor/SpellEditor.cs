using Godot;
using System;

public partial class SpellEditor : GridContainer
{
	[Export]
	public SpellPicker spellPicker;

	[Export]
	public PackedScene spellPieceConfigPanelScene;

	[Export]
	public LineEdit spellNameInput;

	[Export]
	public Button compileButton;

	[Export]
	public Button clearButton;

	

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
		compileButton.Connect("pressed", new Callable(this, nameof(CompileSpell)));
		clearButton.Connect("pressed", new Callable(this, nameof(ClearEditor)));
	}

	
	public override void _Process(double delta)
	{
		if (Input.GetMouseButtonMask() == MouseButtonMask.Left){
			lastMouseButton = LastMouseButton.Left;
		}
		else if (Input.GetMouseButtonMask() == MouseButtonMask.Right){
			lastMouseButton = LastMouseButton.Right;
		}
		else if (Input.GetMouseButtonMask() == MouseButtonMask.Middle){
			lastMouseButton = LastMouseButton.Middle;
		}

		else if (Input.IsActionJustPressed("Cancel"))
		{
			closeExistingPanel();
		}

		if (Input.IsActionJustPressed("EnterN"))
		{
			CompileSpell();
		}
	}

	public SpellPicker getSpellPicker(){
		return spellPicker;
	}

	public void ClearEditor(){
		foreach (SpellEditorBox spellEditorBox in this.GetChildren()){
			spellEditorBox.clear();
		}
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

	public void CompileSpell(){
		SpellEvaluationTreeNode result = null;
		SpellEditorBox rootBox = null;
		
		foreach (SpellEditorBox spellEditorBox in this.GetChildren()){
			if (spellEditorBox.spellPiece is ExecutorSpellPiece){
				rootBox = spellEditorBox;
				break;
			}
		}
		if (rootBox == null){
			SpellWorkspace.showMessage("No valid executor spell piece found");
		}
		

		result = CompileBox(rootBox);
		if (spellNameInput.Text == ""){
			SpellWorkspace.showMessage("No spell name given");
		}
		else if (result == null){
			SpellWorkspace.showMessage("Error in spell compilation");
		}
		else{
			result.PrintTree();
			SpellWorkspace.showMessage("Spell compiled!");
			GameScene.playerSpellStorage.AddSpell(spellNameInput.Text, result);
			SpellWorkspace.Refresh();
		}
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
				case DPad.Direction.NONE:
					SpellWorkspace.showMessage("No parameter direction selected for " + spellEditorBox.selectedSpellPieceName + " at " + getBoxCoordinateInGrid(spellEditorBox));
					return null;
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
