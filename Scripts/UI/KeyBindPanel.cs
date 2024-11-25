using Godot;
using System;
using System.Collections.Generic;

public partial class KeyBindPanel : Panel
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
				}
			}
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	private string[] _lastBindedSpellNames;
	public override void _Process(double delta)
	{
		if (_lastBindedSpellNames != getBindedSpellNames()){
			updatePlayerSpellSet();
			_lastBindedSpellNames = getBindedSpellNames();
		}
		
	}

	private string[] getBindedSpellNames(){
		string[] bindedSpell = new string[actionButtons.Count];
		for (int i = 0; i < actionButtons.Count; i++){
			bindedSpell[i] = actionButtons[i].Text;
		}
		return bindedSpell;
	}

	private List<SpellEvaluationTreeNode> getBindedSpells(){
		// Initialize list with null values for all possible slots
		List<SpellEvaluationTreeNode> bindedSpells = new List<SpellEvaluationTreeNode>();
		for (int i = 0; i < actionButtons.Count; i++) {
			bindedSpells.Add(null);
		}
		
		// Update specific slots based on button selections
		for (int i = 0; i < actionButtons.Count; i++){
			string spellName = actionButtons[i].Text;
			if (spellName != "None"){
				SpellEvaluationTreeNode spell = GameScene.playerSpellStorage.getSpell(spellName);
				bindedSpells[i] = spell;
			}
		}
		return bindedSpells;
	}

	public void updatePlayerSpellSet(){
		GameScene.player.spellCaster.updateSpellSet(getBindedSpells());
	}

	public void refreshOptions(){
		string[] spellNames = GameScene.playerSpellStorage.getSpellNames();

		string[] bindedSpellNames = getBindedSpellNames();
		int i = 0;
		foreach (OptionButton optionButton in actionButtons){
			optionButton.Clear();
			optionButton.AddItem("None", 0);
			int j = 1;
			foreach (string spellName in spellNames){
				optionButton.AddItem(spellName,j);
				if (spellName == bindedSpellNames[i]){
					optionButton.Select(j);
				}
				j++;
			}
			i++;
		}
	}
}
