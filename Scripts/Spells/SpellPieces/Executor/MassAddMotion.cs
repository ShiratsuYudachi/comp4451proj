using Godot;
using System;
using System.Runtime.CompilerServices;

public class MassAddMotion : ExecutorSpellPiece
{
    public override string Name { get { return "MassAddMotion"; } }
    public override SpellVariableType[] ParamList
    {
        get
        {
            return new SpellVariableType[] {
            SpellVariableType.MASSENTITY,
            SpellVariableType.VECTOR2
        };
        }
    }
    public override void Execute(SpellCaster spellCaster, params SpellVariable[] args)
    {
        //checkParams(args);
        IMassEntity entity = args[0].AsMassEntity();
        Vector2 velocity = args[1].AsVector2();
        if ((entity.massVelocity == velocity) || !spellCaster.TryToConsumeMana((int)velocity.Length() * 5 * entity.mass)) return;
        entity.massVelocity = velocity;
    }
}