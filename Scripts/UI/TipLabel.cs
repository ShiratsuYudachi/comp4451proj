using Godot;
using System;
using System.ComponentModel.DataAnnotations.Schema;

public partial class TipLabel : Label
{
	public float timer = 0;

	public float speed = 1f;

	public float getSpeedAtTime(float time){
		return 256 * (float)Math.Exp(-2.42 * time);
	}
	public override void _Process(double delta)
	{
		timer += (float)delta;
		this.Position = new Vector2(Position.X, Position.Y - getSpeedAtTime(timer) * (float)delta);
		if (timer > 2)
		{
			QueueFree();
		}
	}


}
