using Godot;
using System;

public partial class EventManager 
{
	public static Action<bool> PlayerChangeDirection;

	public static Action<int> PlayerSwitchWeapon;
}
