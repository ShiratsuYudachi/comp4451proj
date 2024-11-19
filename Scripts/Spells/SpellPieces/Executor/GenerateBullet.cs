using Godot;
using System;
using System.Runtime.CompilerServices;

public class GenerateBullet : ExecutorSpellPiece
{


    public override SpellVariableType[] ParamList
    {
        get
        {
            return new SpellVariableType[] {
            SpellVariableType.Vector2, // Bullet position
            SpellVariableType.Vector2 // Bullet velocity
        };
        }
    }

    public override SpellVariableType ReturnType { get { return SpellVariableType.MassEntity; } }

    public override void Execute(SpellCaster spellCaster, params SpellVariable[] args)
    {
        //checkParams(args);
        if (!spellCaster.TryToConsumeMana(500)){
            return;
        }
        Vector2 bulletPos = args[0].AsVector2();
        GameScene.ShowSpellAnimation(bulletPos);
        Bullet bullet = GameScene.CreateBullet(bulletPos);
        bullet.caster = spellCaster.GetParent<Entity>();
        if (!spellCaster.TryToConsumeMana((int)(args[1].AsVector2().Length() * 5))){
            return;
        }
        bullet.velocity = args[1].AsVector2();
        
    }
}