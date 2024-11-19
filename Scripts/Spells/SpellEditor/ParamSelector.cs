using Godot;
using System;

public partial class ParamSelector : TextureRect
{
	[Export]
	public Label ParamNameLabel;
	
	[Export]
	public Label ParamTypeLabel;
	[Export]
	public DPad dPad;

	public override void _Ready()
	{
	}

	public void setParamName(string paramName){
		ParamNameLabel.Text = paramName;
	}

	public void setParamType(SpellVariableType paramType){
		ParamTypeLabel.Text = "(" + paramType.ToString() + ")";
	}

	public override void _Process(double delta)
	{
	}
}
