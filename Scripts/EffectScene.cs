using Godot;
using System;

public partial class EffectScene : Node2D
{
	public float timer = 1f;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CpuParticles2D particles = GetNode<CpuParticles2D>("CPUParticles2D");
		particles.Emitting = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		timer -= (float)delta;
		if (timer <= 0)
		{
			QueueFree();
		}
	}
}
