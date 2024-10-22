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
    public override SpellVariableType ReturnType { get { return SpellVariableType.VECTOR2; } }


    public override SpellVariable Select(SpellCaster spellCaster)
    {
        MonsterDetector monsterDetector = GameScene.player.GetNode<MonsterDetector>("MonsterDetector");
        Bullet nearestBullet = null;
        foreach (Bullet bullet in monsterDetector.bulletList){
            if (nearestBullet == null || (bullet.GlobalPosition - spellCaster.GlobalPosition).Length() < (nearestBullet.GlobalPosition - spellCaster.GlobalPosition).Length()){
                nearestBullet = bullet;
            }
        }
        return new SpellVariable(SpellVariableType.MASSENTITY, nearestBullet);
    }	
}