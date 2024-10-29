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
		List<SpellEvaluationTreeNode> bindedSpells = new List<SpellEvaluationTreeNode>();
		foreach (string spellName in getBindedSpellNames()){
			SpellEvaluationTreeNode spell = GameScene.playerSpellStorage.getSpell(spellName);
			if (spell != null){
				bindedSpells.Add(spell);
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
