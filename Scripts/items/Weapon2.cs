using Godot;
using System;

public partial class Weapon2 : Sprite2D
{

	[Export]
	PackedScene bulletScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (this.Visible)
		{
			if (Input.IsActionJustPressed("Attack"))
			{
				Bullet bullet = bulletScene.Instantiate<Bullet>();
				Vector2 mousePosition = GetGlobalMousePosition();
				Vector2 direction = (mousePosition - GlobalPosition).Normalized();
				bullet.Position = GlobalPosition;
				bullet.velocity = direction * 300; 
				bullet.caster = GetTree().GetNodesInGroup("Player")[0] as LivingEntity;
				GameScene.instance.AddChild(bullet);
			}
		}
	}
}
