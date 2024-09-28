using Godot;
using System;
using System.ComponentModel.DataAnnotations.Schema;

public partial class TipLabel : Label
{
	
	[Export]
	public double lifeTime = 2;

	[Export]
	public double speedMultiplier = 256;

	[Export]
	public double speedExponent = 2.42f;

	public double timer = 0;

	public double getSpeedAtTime(double time){
		return speedMultiplier* (double)Math.Exp(-speedExponent * time);
	}
	public override void _Process(double delta)
	{
		timer += (double)delta;
		this.Position = new Vector2(Position.X, (float)(Position.Y - getSpeedAtTime(timer) * (double)delta));
		if (timer > lifeTime)
		{
			QueueFree();
		}
	}


}
