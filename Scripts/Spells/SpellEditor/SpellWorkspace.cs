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
		GetNode<TriggerBindPanel>("TriggerBindPanel").refreshOptions();
		GetNode<StoredSpellsList>("StoredSpellsList").refresh();
	}
	public static void Refresh(){
		instance.refresh();
	}

	private static string[] messageHistory = new string[5];
	private static int messageIndex = 0;

	public static void showMessage(string message){
		// Store message in history array, keep only last 3 messages
		messageHistory[messageIndex] = message;
		messageIndex = (messageIndex + 1) % 3;

		// Build display string with latest 3 messages
		string displayText = "";
		for(int i = 0; i < 3; i++) {
			int idx = (messageIndex - i - 1 + 3) % 3;
			if(messageHistory[idx] != null) {
				if(displayText != "") displayText += "\n";
				displayText += messageHistory[idx];
			}
		}

		var label = instance.GetNode<Label>("Message");
		label.Text = displayText;
		// Hide message after 7 seconds
		label.GetTree().CreateTimer(7.0).Timeout += () => label.Text = "";
	}

}
