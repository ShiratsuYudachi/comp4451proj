using Godot;
using System;

public abstract partial class LivingEntity : CharacterBody2D
{
	
	protected AnimatedSprite2D animatedSprite2D;

	[Export]
	protected PackedScene deathEffectScene;
	

	protected enum State
	{
		Idle,
		Moving
	}

	protected float timer = 3;
	protected State state = State.Idle;

	protected Vector2 randomDirection;
	protected bool isRight = true;
	
	public int MAX_HEALTH = 100;
	protected int health = 100;
	protected float hitFlashDuration = 0.2f;
	protected float hitFlashTimer = 0f;

    



	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        if (animatedSprite2D == null)
        {
            GD.PushError("No AnimatedSprite2D node found!");
			GetTree().Quit();
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (hitFlashTimer > 0)
		{
			hitFlashTimer -= (float)delta;
			if (hitFlashTimer <= 0)
			{
				animatedSprite2D.Modulate = Colors.White;
			}
		}
	}

	public virtual void OnHit(int damage)
	{
		health -= damage;
		GD.Print("Enemy hit for " + damage + " damage. Remaining health: " + health);

		// 触发受击闪烁效果
		animatedSprite2D.Modulate = Colors.Red;
		hitFlashTimer = hitFlashDuration;

		GameScene.ShowDamage(damage, GlobalPosition);

		if (health <= 0)
		{
			Die();
		}
	}

	public virtual void Die()
	{
		TipManager.ShowTip("Enemy defeated!");

		// 这里可以添加死亡动画、掉落物品等逻辑		
		if (deathEffectScene != null)
		{
			Node2D effect = deathEffectScene.Instantiate<Node2D>();
			GetTree().Root.AddChild(effect);
			effect.GlobalPosition = GlobalPosition;
		}

		QueueFree(); 
	}
}
