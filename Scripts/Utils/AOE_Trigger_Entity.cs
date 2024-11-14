using System;
using Godot;

public partial class AOE_Trigger_Entity: Area2D
{
    public Action<Entity> OnTrigger;

    

    public override void _Ready(){
        this.AreaEntered += _on_area_entered;
    }

    public void SetRadius(float radius){
        CollisionShape2D collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
        collisionShape.Shape = new CircleShape2D{Radius = radius};
    }

    // public void SetTargetType(Type targetType){
    //     _targetType = targetType;
    // }

    private void _on_area_entered(Area2D area){
        Node2D parent = area.GetParent<Node2D>();
        if (parent is Entity entity){
            OnTrigger?.Invoke(entity);
        }
    }

}