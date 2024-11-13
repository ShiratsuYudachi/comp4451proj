using Godot;
using System;
public partial class WizardTower : MapEntity
{
    [Export] private PackedScene bulletScene;
    private const float ATTACK_INTERVAL = 2.0f;
    private float attackTimer = ATTACK_INTERVAL;
    private int NUM_BULLETS = 3;
    public override void _Ready()
    {
        MAX_HEALTH = 100;
        health = MAX_HEALTH;
        group = Group.Enemy;
        base._Ready();
        hardness = 10;
        animatedSprite2D.Play("default");
    }
    public override void _Process(double delta)
    {
        attackTimer -= (float)delta;
        if (attackTimer <= 0)
        {
            Godot.Vector2 bulletGlobalPosition = GlobalPosition;
            for (int i = 0; i < NUM_BULLETS; ++i)
            {
                Bullet bullet = bulletScene.Instantiate<Bullet>();
                bullet.caster = this;
                bullet.GlobalPosition = bulletGlobalPosition;
                Node2D player = GameScene.player;
                Vector2 direction = (player.GlobalPosition - GlobalPosition).Normalized();
                bullet.velocity = direction * 100;
                GameScene.instance.AddChild(bullet);
                bulletGlobalPosition -= direction * 10;
                attackTimer = ATTACK_INTERVAL;
            }
        }
        base._Process(delta);
    }
    public override void OnHit(int damage)
    {
        base.OnHit(damage);
    }
    public override void Die()
    {
        base.Die();
    }
    public override void ApplyDamage(long amout = 0, Vector2? direction = null, Entity source = null)
    {
        throw new NotImplementedException();
    }
}