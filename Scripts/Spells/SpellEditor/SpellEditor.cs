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
	}

	public SpellPicker getSpellPicker(){
		return spellPicker;
	}



	public void setSpellPieceConfigPanelAt(SpellEditorBox spellEditorBox){
		if (spellEditorBox.spellPiece == null) return;
		if (spellPieceConfigPanel != null && IsInstanceValid(spellPieceConfigPanel)){
			spellPieceConfigPanel.QueueFree();
		}
		spellPieceConfigPanel = spellPieceConfigPanelScene.Instantiate<SpellPieceConfigPanel>();
		this.GetParent().AddChild(spellPieceConfigPanel);
		spellPieceConfigPanel.Position = spellEditorBox.Position*this.Scale + this.Position;
		spellPieceConfigPanel.setupParamSelectorsAt(spellEditorBox);

		spellPicker.resetPosition();
	}

	public void setSpellPickerAt(SpellEditorBox spellEditorBox){
		spellPicker.Position = spellEditorBox.Position*this.Scale + this.Position;
		if (spellPieceConfigPanel != null && IsInstanceValid(spellPieceConfigPanel)){
			spellPieceConfigPanel.resetPosition();
		}
	}



	// public TextureRect NewSpellPieceIcon(string name){
	// 	TextureRect icon = spellPieceIcon.Instantiate<TextureRect>();
	// 	icon.Texture = GD.Load<Texture2D>("res://images/spells/"+name+".png");
	// 	return icon;
	// }
}
