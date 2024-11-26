using Godot;
public partial class WaterZone : Area2D
{
    public override void _Ready()
    {
        base._Ready();
        Connect("area_entered", new Callable(this, nameof(OnAreaEntered)));
    }
    private void OnAreaEntered(Area2D area)
    {
        if (!area.IsInGroup("HitBox")) return;
        Entity entity = area.GetParent() as Entity;
        entity?.OnHit(0, null, null, Chemistry.Element.Hydro, 10);
    }
}