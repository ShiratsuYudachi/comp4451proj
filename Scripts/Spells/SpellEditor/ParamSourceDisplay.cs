using Godot;
using System;

public partial class ParamSourceDisplay : Control
{
	TextureRect SourceUp;
	TextureRect SourceDown;
	TextureRect SourceLeft;
	TextureRect SourceRight;

	public override void _Ready()
	{
		SourceUp = GetNode<TextureRect>("SourceUp");
		SourceDown = GetNode<TextureRect>("SourceDown");
		SourceLeft = GetNode<TextureRect>("SourceLeft");
		SourceRight = GetNode<TextureRect>("SourceRight");
		SourceUp.Visible = false;
		SourceDown.Visible = false;
		SourceLeft.Visible = false;
		SourceRight.Visible = false;
	}

	public void updateParamSourceDisplay(DPad.Direction[] SpellPieceParamDirection){
		SourceUp.Visible = false;
		SourceDown.Visible = false;
		SourceLeft.Visible = false;
		SourceRight.Visible = false;
		foreach (DPad.Direction direction in SpellPieceParamDirection){
			if (direction == DPad.Direction.UP){
				SourceUp.Visible = true;
			}
			if (direction == DPad.Direction.DOWN){
				SourceDown.Visible = true;
			}
			if (direction == DPad.Direction.LEFT){
				SourceLeft.Visible = true;
			}
			if (direction == DPad.Direction.RIGHT){
				SourceRight.Visible = true;
			}
		}
	}
}
