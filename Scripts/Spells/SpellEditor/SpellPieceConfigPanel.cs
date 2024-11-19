using Godot;
using System;

public partial class SpellPieceConfigPanel : Control
{
	[Export]
	public PackedScene paramSelector;
	[Export]
	public PackedScene configVector2Panel;
	[Export]
	public PackedScene configIntPanel;

	public Vector2 spellPieceConfigPanelInitialPosition;

	public Button doneButton;

	public SpellEditorBox editorBoxAttached;
	public ParamSelector[] paramSelectors;
	public ConfigItem[] configItems;
	public override void _Ready()
	{
		spellPieceConfigPanelInitialPosition = this.Position;
		doneButton = GetNode<Button>("DoneButton");
		doneButton.Connect("button_down", new Callable(this, "doneButtonPressed"));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void resetPosition()
	{
		this.Position = spellPieceConfigPanelInitialPosition;
	}


	public void setupParamSelectorsAt(SpellEditorBox spellEditorBox)
	{
		editorBoxAttached = spellEditorBox;
		// create config items
		configItems = new ConfigItem[spellEditorBox.spellPiece.ConfigList.Length];

		for (int i = 0; i < spellEditorBox.spellPiece.ConfigList.Length; i++)
		{
			SpellVariableType type = spellEditorBox.spellPiece.ConfigList[i];
			
			ConfigItem newConfigItem = null;
			switch (type)
			{
				case SpellVariableType.Vector2:
					newConfigItem = configVector2Panel.Instantiate<ConfigVectorConstant>();
					break;
				case SpellVariableType.INT:
					newConfigItem = configIntPanel.Instantiate<ConfigIntConstant>();
					break;
			}
			string configName = SpellRegistry.GetSpellPieceInfo(spellEditorBox.spellPiece.GetType().Name).getConfigName(i);
			

			if (configName != "")
				newConfigItem.setConfigName(configName);
			
			this.GetNode<Control>("VAlign").AddChild(newConfigItem);
			configItems[i] = newConfigItem;
		}

		// create param direction selectors
		paramSelectors = new ParamSelector[spellEditorBox.spellPiece.ParamList.Length];

		for (int i = 0; i < spellEditorBox.spellPiece.ParamList.Length; i++)
		{
			SpellVariableType type = spellEditorBox.spellPiece.ParamList[i];
			ParamSelector newParamSelector = paramSelector.Instantiate<ParamSelector>();

			newParamSelector.setParamName(SpellRegistry.GetSpellPieceInfo(spellEditorBox.spellPiece.GetType().Name).getParamName(i, type.ToString()));
			newParamSelector.setParamType(type);
			this.GetNode<Control>("VAlign").AddChild(newParamSelector);
			paramSelectors[i] = newParamSelector;
		}

		// load config items
		for (int i = 0; i < spellEditorBox.spellPiece.ConfigList.Length; i++)
		{
			configItems[i].parseConfigValue(spellEditorBox.spellPiece.getConfigValues()[i]);
		}

		// load parameter
		for (int i = 0; i < spellEditorBox.spellPiece.ParamList.Length; i++)
		{
			paramSelectors[i].dPad.SetDirection(spellEditorBox.SpellPieceParamDirection[i]);
		}
	}

	public void doneButtonPressed()
	{
		writeParamDirectionToSpellPiece();
		object[] configValues = new object[configItems.Length];
		for (int i = 0; i < configItems.Length; i++)
		{
			configValues[i] = configItems[i].getConfigValue();
		}
		editorBoxAttached.spellPiece.applyConfig(configValues);
		QueueFree();
	}

	public void writeParamDirectionToSpellPiece()
	{
		for (int i = 0; i < editorBoxAttached.spellPiece.ParamList.Length; i++)
		{
			editorBoxAttached.SpellPieceParamDirection[i] = paramSelectors[i].dPad.GetDirection();
		}
		editorBoxAttached.paramSourceDisplay.updateParamSourceDisplay(editorBoxAttached.SpellPieceParamDirection);
	}
}
