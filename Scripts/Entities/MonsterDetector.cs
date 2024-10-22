using System.Collections.Generic;
using Godot;

public partial class MonsterDetector : Area2D
{
    List<LivingEntity> monsterList = new List<LivingEntity>();
    public List<Bullet> bulletList = new List<Bullet>();
    private CharacterBody2D Player;
    public override void _Ready()
    {
        Player = GetParent<CharacterBody2D>();
        BodyShapeEntered += OnBodyShapeEntered;
        BodyShapeExited += OnBodyShapeExited;
        this.Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
        this.Connect("area_exited", new Callable(this, nameof(OnAreaExited)));
    }
    private void OnAreaEntered(Area2D area){
        if (area.GetParent() is Bullet){
            bulletList.Add((Bullet)area.GetParent());
        }
    }
    private void OnAreaExited(Area2D area){
        if (area.GetParent() is Bullet){
            bulletList.Remove((Bullet)area.GetParent());
        }
    }
    private void OnBodyShapeEntered(Rid bodyRid, Node2D body, long bodyShapeIndex, long areaShapeIndex)
    {
        if (body is LivingEntity && body != Player)
        {
            monsterList.Add((LivingEntity)body);
        }
        
    }
    private void OnBodyShapeExited(Rid bodyRid, Node2D body, long bodyShapeIndex, long areaShapeIndex)
    {
        if (body is LivingEntity && body != Player)
        {
            monsterList.Remove((LivingEntity)body);
        }   
    }
}