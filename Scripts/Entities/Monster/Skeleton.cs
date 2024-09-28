using Godot;
using System;

public partial class Skeleton : LivingEntity
{
    [Export]
    private PackedScene bulletScene;

	public override void _Ready()
	{
		base._Ready();
        this.animatedSprite2D.Modulate = Colors.White;
	}

    public float ATTACK_INTERVAL = 1; //s
    private float attackTimer = 0f;


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
        attackTimer -= (float)delta;
        if (attackTimer <= 0)
        {
            Attack();
            attackTimer = ATTACK_INTERVAL; // 重置攻击计时器
        }
		base._Process(delta);
	}

	public void Attack()
	{
		Bullet bullet = bulletScene.Instantiate<Bullet>();
		
        Node2D player = GetTree().GetNodesInGroup("Player")[0] as Node2D;
        Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
        bullet.GlobalPosition = GlobalPosition;
        bullet.velocity = direction * 100;
		bullet.caster = this;
		GetTree().Root.AddChild(bullet);
	}

	public override void OnHit(int damage)
	{
		
		base.OnHit(damage);
	}

	public override void Die()
	{
		base.Die();
	}
}
