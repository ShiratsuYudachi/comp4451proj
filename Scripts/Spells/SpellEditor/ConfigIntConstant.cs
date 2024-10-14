using Godot;
using System;

public partial class ConfigIntConstant : ConfigItem
{
    TextEdit textEdit;
    public override void _Ready()
    {
        textEdit = GetNode<Control>("Input").GetNode<TextEdit>("TextEdit");

    }
    public override object getConfigValue()
    {
        return int.Parse(textEdit.Text);
    }
    public override void parseConfigValue(object value)
    {
        textEdit.Text = ((int)value).ToString();
    }
}