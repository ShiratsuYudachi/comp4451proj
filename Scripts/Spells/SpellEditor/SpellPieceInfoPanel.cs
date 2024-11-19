using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SpellPieceInfoPanel : Panel
{
	[Export]
	public Label spellPieceNameLabel;
	[Export]
	public Label descriptionLabel;
	[Export]
	public Label inputContentsLabel;
	[Export]
	public Label outputContentsLabel;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		this.Hide();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (this.Visible){
			this.GlobalPosition = GetGlobalMousePosition();
		}
	}


	public void setInfo(string spellPieceName){
		spellPieceNameLabel.Text = spellPieceName;
		SpellPieceInfo pieceInfo = SpellRegistry.GetSpellPieceInfo(spellPieceName);
		descriptionLabel.Text = pieceInfo.description;
		SpellPiece spellPiece = (SpellPiece)Activator.CreateInstance(pieceInfo.spellClassType);
		List<string> inputTypes = spellPiece.ParamList.Select(x => x.ToString()).ToList();
		List<string> inputNames = pieceInfo.paramNames;
		outputContentsLabel.Text = spellPiece.ReturnType.ToString();
		string inputContent = "";
		if (inputNames != null && inputNames.Count > 0){
			for(int i = 0; i < inputTypes.Count; i++){
				inputContent += inputNames[i] + "(" + inputTypes[i] + ")";
				if(i != inputTypes.Count - 1){
					inputContent += ", ";
				}
			}
		}else if (inputTypes != null && inputTypes.Count > 0){
			inputContent = string.Join(", ", inputTypes);
		}else{
			inputContent = "NONE";
		}
		inputContentsLabel.Text = inputContent;
	}
}
