using Godot;
using System;
using System.Collections.Generic;

public partial class TriggerBindPanel : Panel
{
	// Called when the node enters the scene tree for the first time.
	List<OptionButton> actionButtons = new List<OptionButton>();
	public override void _Ready()
	{
		foreach (Control child in this.GetChildren()){
			if (child is ReferenceRect){
				OptionButton optionButton = child.GetNode<OptionButton>("OptionButton");
				if (optionButton != null){
					actionButtons.Add(optionButton);
					optionButton.Connect("item_selected", new Callable(this, nameof(updatePlayerTriggerSet)));
				}
				
			}
		}
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	
	public override void _Process(double delta)
	{
	}

    private string[] getBindedTriggerNames(){
		string[] bindedTrigger = new string[actionButtons.Count];
		for (int i = 0; i < actionButtons.Count; i++){
			bindedTrigger[i] = actionButtons[i].Text;
		}
		return bindedTrigger;
	}

	public void updatePlayerTriggerSet(int index){
		if (getBindedTriggerNames().Length > 0 && getBindedTriggerNames()[0] != "None" && getBindedTriggerNames()[0]!=null){
			// GD.Print("getBindedTriggerNames()[0]: " + getBindedTriggerNames()[0]);
			GameScene.player.spellCaster.updateTriggerSet(getBindedTriggerNames()); 
		}
	}
    
    public void refreshOptions(){
		string[] spellNames = GameScene.playerSpellStorage.getSpellNames();

		string[] bindedTriggerNames = getBindedTriggerNames();
		int i = 0;
		foreach (OptionButton optionButton in actionButtons){
			optionButton.Clear();
			optionButton.AddItem("None", 0);
			int j = 1;
			foreach (string spellName in spellNames){
				optionButton.AddItem(spellName,j);
				if (spellName == bindedTriggerNames[i]){
					optionButton.Select(j);
				}
				j++;
			}
			i++;
		}
	}
}
