using Godot;
using System;

public partial class StoredSpellsList : ItemList
{
	[Export]
	public SpellEditor spellEditor;

	public override void _Ready()
	{
		this.ItemSelected += (index) => {
			string spellName = this.GetItemText((int)index);
			spellEditor.LoadWorkspace(spellName);
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void refresh(){
		this.Clear();
		foreach (string spellName in GameScene.playerSpellStorage.getSpellNames()){
			this.AddItem(spellName);
		}
	}
}



