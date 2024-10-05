using Godot;
using System;

public abstract partial class ConfigItem : Control
{
	public abstract object getConfigValue(); // for getting the value from the GUI
	public abstract void parseConfigValue(object value); // for parsing the value from the spell piece, and setting the GUI
};


public partial class ConfigVectorConstant : ConfigItem
{	
	TextEdit xTextEdit;
	TextEdit yTextEdit;

	public override void _Ready(){
		xTextEdit = GetNode<Control>("Input").GetNode<Control>("X").GetNode<TextEdit>("TextEdit");
		yTextEdit = GetNode<Control>("Input").GetNode<Control>("Y").GetNode<TextEdit>("TextEdit");
	}

	public override object getConfigValue(){
		return new Vector2(float.Parse(xTextEdit.Text), float.Parse(yTextEdit.Text));
	}

	public override void parseConfigValue(object value){
		Vector2 vec = (Vector2)value;
		xTextEdit.Text = vec.X.ToString();
		yTextEdit.Text = vec.Y.ToString();
	}
}
