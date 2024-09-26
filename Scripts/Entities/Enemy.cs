using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
	// Called when the node enters the scene tree for the first time.
	AnimatedSprite2D animatedSprite2D;
	
	[Export]
	private PackedScene effectScene;

	private enum State
	{
		Idle,
		Moving
	}

	private float timer = 3;
	private State state = State.Idle;

	private Vector2 randomDirection;
	private bool isRight = true;
	
	private int health = 100;
	private float hitFlashDuration = 0.2f;
	private float hitFlashTimer = 0f;



	public override void _Ready()
	{
		animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		switch (state)
		{
			case State.Idle:
				animatedSprite2D.Play("idle");
				timer -= (float)delta;
				if (timer <= 0)
				{
					state = State.Moving;
					randomDirection = new Vector2(
						(float)GD.RandRange(-1, 1),
						(float)GD.RandRange(-1, 1)
					).Normalized();
					timer = 1;
				}
				break;
			case State.Moving:
				animatedSprite2D.Play("moving");
				Velocity = randomDirection * 50;
				MoveAndSlide();

				// 添加转向逻辑
				if (randomDirection.X > 0) 
				{
					isRight = true;
				}
				else if (randomDirection.X < 0)
				{
					isRight = false;
				}
				animatedSprite2D.FlipH = !isRight;

				timer -= (float)delta;
				if (timer <= 0)
				{
					Velocity = Vector2.Zero;
					state = State.Idle;
					timer = 3;
					randomDirection = new Vector2(
						(float)GD.RandRange(-1, 1),
						(float)GD.RandRange(-1, 1)
					).Normalized();
					timer = 1;
				}
				break;
		}

		// 处理受击闪烁效果
		if (hitFlashTimer > 0)
		{
			hitFlashTimer -= (float)delta;
			if (hitFlashTimer <= 0)
			{
				animatedSprite2D.Modulate = Colors.White;
			}
		}
	}

	public void OnHit(int damage)
	{
		health -= damage;
		GD.Print("Enemy hit for " + damage + " damage. Remaining health: " + health);

		// 触发受击闪烁效果
		animatedSprite2D.Modulate = Colors.Red;
		hitFlashTimer = hitFlashDuration;

		if (health <= 0)
		{
			Die();
		}
	}

	private void Die()
	{
		GD.Print("Enemy defeated!");
		TipManager.ShowTip("Enemy defeated!");
		// 这里可以添加死亡动画、掉落物品等逻辑
		Node2D effect = effectScene.Instantiate<Node2D>();
		GetTree().Root.AddChild(effect);
		effect.GlobalPosition = GlobalPosition;
		QueueFree(); // 从场景中移除敌人
	}
}
