using Godot;
using System;

public partial class UIControl : Control
{
	// Called when the node enters the scene tree for the first time.
	TextureButton _weapon1Button;
	TextureButton _weapon2Button;
	public override void _Ready()
	{
		_weapon1Button = GetNode<TextureButton>("Weapon1Button");
		_weapon2Button = GetNode<TextureButton>("Weapon2Button");
 
		_weapon1Button.Connect("pressed", new Callable(this, "OnWeapon1ButtonDown"));
		_weapon2Button.Connect("pressed", new Callable(this, "OnWeapon2ButtonDown"));
	}

	public void OnWeapon1ButtonDown()
	{
		EventManager.PlayerSwitchWeapon?.Invoke(1);
	}

	public void OnWeapon2ButtonDown()
	{
		EventManager.PlayerSwitchWeapon?.Invoke(2);
	}
}
