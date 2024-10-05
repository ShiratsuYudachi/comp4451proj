using Godot;
using System;

public partial class ParamSelector : TextureRect
{
	[Export]
	public Label ParamNameLabel;

	[Export]
	public DPad dPad;

	public override void _Ready()
	{
	}

	public void setParamName(string paramName){
		ParamNameLabel.Text = paramName;
	}

	public override void _Process(double delta)
	{
	}
}
