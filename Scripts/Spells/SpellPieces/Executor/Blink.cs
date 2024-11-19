using Godot;
using System;
using System.Runtime.CompilerServices;

public class Blink : ExecutorSpellPiece
{
    public override string Name
    {
        get
        {
            return "Blink";
        }
    }


    public override SpellVariableType[] ParamList
    {
        get
        {
            return new SpellVariableType[] {
            SpellVariableType.MassEntity, // Entity to blink
            SpellVariableType.Vector2 // Blink target relative to entity
        };
        }
    }

    public override void Execute(SpellCaster spellCaster, params SpellVariable[] args)
    {
        //checkParams(args);
        IMassEntity entity = args[0].AsMassEntity();
        Vector2 targetRelativePos = args[1].AsVector2();
        if (!spellCaster.TryToConsumeMana((int)targetRelativePos.Length() * 5 * entity.mass)) return;
        GameScene.ShowSpellAnimation(entity.massPosition);
        GameScene.ShowSpellAnimation(entity.massPosition + targetRelativePos);
        entity.massPosition += targetRelativePos;
    }
}