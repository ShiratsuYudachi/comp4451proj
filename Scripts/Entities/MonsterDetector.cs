using System.Collections.Generic;
using Godot;

public partial class MonsterDetector : Area2D
{
    List<LivingEntity> monsterList = new List<LivingEntity>();
    private CharacterBody2D Player;
    public override void _Ready()
    {
        Player = GetParent<CharacterBody2D>();
        BodyShapeEntered += OnBodyShapeEntered;
        BodyShapeExited += OnBodyShapeExited;
    }
    private void OnBodyShapeEntered(Rid bodyRid, Node2D body, long bodyShapeIndex, long areaShapeIndex)
    {
        if (body is LivingEntity && body != Player)
        {
            monsterList.Add((LivingEntity)body);
            GD.Print($"Add monster {bodyRid}");
        }
    }
    private void OnBodyShapeExited(Rid bodyRid, Node2D body, long bodyShapeIndex, long areaShapeIndex)
    {
        if (body is LivingEntity && body != Player)
        {
            monsterList.Remove((LivingEntity)body);
            GD.Print($"Delete monster {bodyRid}");
        }
    }
}