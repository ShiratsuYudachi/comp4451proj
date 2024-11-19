using Godot;
using System;
using System.Runtime.CompilerServices;

public class MassAddMotion : ExecutorSpellPiece
{
    public override SpellVariableType[] ParamList
    {
        get
        {
            return new SpellVariableType[] {
            SpellVariableType.MassEntity,
            SpellVariableType.Vector2
        };
        }
    }
    public override void Execute(SpellCaster spellCaster, params SpellVariable[] args)
    {
        //checkParams(args);
        IMassEntity entity = args[0].AsMassEntity();
        Vector2 velocity = args[1].AsVector2();
        if ((entity.massVelocity == velocity) || !spellCaster.TryToConsumeMana((int)velocity.Length() * 5 * entity.mass)) return;
        GameScene.ShowSpellAnimation(entity.massPosition);
        entity.ApplyMotion(velocity);
    }
}