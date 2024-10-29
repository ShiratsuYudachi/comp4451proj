using Godot;
using System;
using System.Runtime.CompilerServices;

public class GenerateBullet : OperatorSpellPiece
{
    public override string Name
    {
        get
        {
            return "GenerateBullet";
        }
    }


    public override SpellVariableType[] ParamList
    {
        get
        {
            return new SpellVariableType[] {
            SpellVariableType.VECTOR2 // Bullet position
        };
        }
    }

    public override SpellVariableType ReturnType { get { return SpellVariableType.MASSENTITY; } }

    public override SpellVariable Operate(SpellCaster spellCaster, params SpellVariable[] args)
    {
        //checkParams(args);
        Vector2 bulletPos = args[0].AsVector2();
        GameScene.ShowSpellAnimation(bulletPos);
        Bullet bullet = GameScene.CreateBullet(bulletPos);
        bullet.caster = spellCaster.GetParent<Entity>();
        return new SpellVariable(SpellVariableType.MASSENTITY, bullet);
    }
}