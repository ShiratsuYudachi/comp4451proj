using Godot;
using System;

public partial class SpellWorkspace : Control
{
	public static SpellWorkspace instance;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void refresh(){
		GetNode<KeyBindPanel>("KeyBindPanel").refreshOptions();
		GetNode<StoredSpellsList>("StoredSpellsList").refresh();
	}
	public static void Refresh(){
		instance.refresh();
	}

	public void showMessage(string message){
		GetNode<Label>("Message").Text = message;
	}

}
