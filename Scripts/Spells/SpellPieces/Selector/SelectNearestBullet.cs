using Godot;
using System;
using System.Runtime.CompilerServices;

public class SelectNearestBullet : SelectorSpellPiece
{
	public override string Name
    {
        get 
        { 
            return "Select Nearest Bullet"; 
        }
    }
    public override SpellVariableType ReturnType { get { return SpellVariableType.MassEntity; } }
    
    public override SpellVariableType[] ConfigList { get { return new SpellVariableType[] { SpellVariableType.BOOL }; } }
	
    public bool avoidDuplicate = false; // If true, the selector will avoid selecting the same entity consecutively

    private CircularBuffer<IMassEntity> lastSelectedEntities = new CircularBuffer<IMassEntity>(20);
	public override void applyConfig(object[] configs)
	{
		avoidDuplicate = (bool)configs[0];
	}

	public override object[] getConfigValues()
	{
		return new object[] { avoidDuplicate };
	}


    public override SpellVariable Select(SpellCaster spellCaster)
    {
        EntityDetector entityDetector = GameScene.player.GetNode<EntityDetector>("EntityDetector");
        if (entityDetector == null){
            GD.PrintErr("SelectNearestBullet failed: No EntityDetector found");
            return new SpellVariable(SpellVariableType.NONE, null);
        }

        Bullet nearestBullet = null;
        
        foreach (IMassEntity massEntity in entityDetector.entityList){
            
            if (massEntity is Bullet){
                if (massEntity != null && 
                    (nearestBullet == null || 
                    (massEntity.massPosition - spellCaster.GlobalPosition).Length() < (nearestBullet.massPosition - spellCaster.GlobalPosition).Length())){
                    nearestBullet = (Bullet)massEntity;
                }
            }
        }

        if (avoidDuplicate && lastSelectedEntities.Contains(nearestBullet)){
            nearestBullet = null;
        }
        
        if (nearestBullet is null){
            GameScene.inGameLog("SelectNearestBullet failed: No bullet found");
            return new SpellVariable(SpellVariableType.NONE, null);
        }
        //entityDetector.entityList.Remove(nearestBullet);
        lastSelectedEntities.Add(nearestBullet);
        return new SpellVariable(SpellVariableType.MassEntity, nearestBullet);
        
    }	
}