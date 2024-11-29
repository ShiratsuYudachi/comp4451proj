using Godot;
using System.Collections.Generic;

public partial class WaterZone : Area2D
{
    // List to store entities in water zone
    private List<Entity> _entitiesInWater = new List<Entity>();
    
    private double _timer = 0.0;
    private const double HYDRO_INTERVAL = 2.0; // Apply hydro every 3 seconds

    public override void _Ready()
    {
        base._Ready();
        Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
        Connect("area_exited", new Callable(this, nameof(OnAreaExited)));
    }

    public override void _Process(double delta)
    {
        base._Process(delta);
        
        _timer += delta;
        if (_timer >= HYDRO_INTERVAL)
        {
            _timer = 0.0; // Reset timer
            foreach (var entity in _entitiesInWater)
            {
                entity.reactor.AddElement(Chemistry.Element.Hydro, 10);
            }
        }
    }

    private void OnAreaEntered(Area2D area)
    {
        if (!area.IsInGroup("HitBox")) return;
        
        Entity? entity = area.GetParent() as Entity;
        // Check if entity is null or if its reactor is null
        if (entity == null || entity.reactor == null) return;
        
        // Add entity to list and apply initial hydro
        _entitiesInWater.Add(entity);
        entity.reactor.AddElement(Chemistry.Element.Hydro, 10);
    }

    private void OnAreaExited(Area2D area)
    {
        if (!area.IsInGroup("HitBox")) return;
        
        Entity? entity = area.GetParent() as Entity;
        // Check if entity is null
        if (entity == null) return;
        
        // Remove entity from list
        _entitiesInWater.Remove(entity);
    }
}