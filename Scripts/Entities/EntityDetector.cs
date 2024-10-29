using System.Collections.Generic;
using Godot;

public partial class EntityDetector : Area2D
{
    public List<IMassEntity> entityList = new List<IMassEntity>();
    
    private CharacterBody2D Player;
    public override void _Ready()
    {
        Player = GetParent<CharacterBody2D>();
        //BodyShapeEntered += OnBodyShapeEntered;
        //BodyShapeExited += OnBodyShapeExited;
        this.Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
        this.Connect("area_exited", new Callable(this, nameof(OnAreaExited)));
    }
    private void OnAreaEntered(Area2D area){
        if (!area.IsInGroup("HitBox")) return;
        
        IMassEntity massEntity = area.GetParent() as IMassEntity;
        if (massEntity != null && !entityList.Contains(massEntity)){
            // GD.Print("EntityDetector: " + massEntity.GetType().Name + " added to entityList");
            entityList.Add(massEntity);
        }
    }
    private void OnAreaExited(Area2D area){
        if (!area.IsInGroup("HitBox")) return;
        IMassEntity massEntity = area.GetParent() as IMassEntity;
        if (massEntity != null && entityList.Contains(massEntity)){
            // GD.Print("EntityDetector: " + massEntity.GetType().Name + " removed from entityList");
            entityList.Remove(massEntity);
        }
    }
}