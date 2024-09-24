using Godot;
using System;

public partial class WeaponControl : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		EventManager.PlayerChangeDirection += (bool isRight) =>
		{
			foreach (Sprite2D weapon in this.GetChildren())
			{
				weapon.FlipH = !isRight;
			}
		};

		EventManager.PlayerSwitchWeapon += (int weaponIndex) =>
		{
			switch (weaponIndex)
			{
				case 1:
					this.GetNode<Sprite2D>("Weapon1").Visible = true;
					this.GetNode<Sprite2D>("Weapon2").Visible = false;
					break;
				case 2:
					this.GetNode<Sprite2D>("Weapon1").Visible = false;
					this.GetNode<Sprite2D>("Weapon2").Visible = true;
					break;
			}
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
