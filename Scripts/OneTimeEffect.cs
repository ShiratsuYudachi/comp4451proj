using Godot;
using System;

public partial class OneTimeEffect : Node2D
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CpuParticles2D particles = GetNode<CpuParticles2D>("CPUParticles2D");
		particles.Emitting = true;
		particles.Connect("finished", new Callable(this, nameof(onEffectEnd)));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{


	}

	public void onEffectEnd(){
		QueueFree();
	}
}
