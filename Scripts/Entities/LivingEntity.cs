#nullable enable
using Godot;
using System;
public abstract partial class LivingEntity : CharacterBody2D, IDamageable
{
	protected AnimatedSprite2D? animatedSprite2D;
	[Export]
	protected PackedScene? deathEffectScene;
	protected enum State { Idle, Moving }
	protected float timer = 3;
	protected State state = State.Idle;
	protected Vector2 randomDirection;
	protected bool isRight = true;
	protected int health = 100;
	public int Health
	{
		get { return health; }
		set { health = value; }
	}
	protected const int MAX_HEALTH = 100;
	public int MaxHealth { get { return MAX_HEALTH; } }
	public Vector2 speed;
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
				if (animatedSprite2D == null)
				{
					GD.Print("No AnimatedSprite2D found!");
					GetTree().Quit();
				}
				else
				{
					animatedSprite2D.Modulate = Colors.White;
				}
			}
		}
		Vector2 offset = new Vector2((float)(speed.X * delta), (float)(speed.Y * delta));
		Position += offset;
	}
	public void ApplyDamage(long amount = 0L, Vector2? direction = null, Node2D? source = null)
	{
		direction = direction ?? Vector2.Zero;
		//TODO
	}
	public virtual void OnHit(int damage)
	{
		health -= damage;
		GD.Print("Enemy hit for " + damage + " damage. Remaining health: " + health);
		// 触发受击闪烁效果
		if (animatedSprite2D == null)
		{
			GD.Print("No AnimatedSprite2D found!");
			GetTree().Quit();
		}
		else
		{
			animatedSprite2D.Modulate = Colors.Red;
		}
		hitFlashTimer = hitFlashDuration;
		GameScene.ShowDamage(damage, GlobalPosition);
		if (health <= 0) Die();
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
