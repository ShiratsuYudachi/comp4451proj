using Godot;
using System;

public partial class Ui : Control
{
	[Export]
	public PackedScene spellEditorScene;
	public SpellWorkspace spellEditor;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		spellEditor = GetNode<SpellWorkspace>("SpellWorkspace");
		hideMenu();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.

	private void showMenu(){
		spellEditor.Visible = true;
	}
	private void hideMenu(){
		spellEditor.Visible = false;
	}
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("ShowSpellEditor")){
			if (spellEditor.Visible){
				hideMenu();
				this.GetTree().Paused = false;
			}else{
				showMenu();
				this.GetTree().Paused = true;
				spellEditor.refresh();
			}
			
			
		}

	}
}
