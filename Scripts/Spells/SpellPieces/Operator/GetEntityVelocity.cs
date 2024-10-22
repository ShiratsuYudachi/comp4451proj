using Godot;
using System;
using System.Runtime.CompilerServices;

public class GetEntityVelocity : OperatorSpellPiece
{
    public override string Name
    {
        get
        {
            return "Get Entity Velocity";
        }
    }

    public override SpellVariableType[] ParamList
    {
        get
        {
            return new SpellVariableType[] {
            SpellVariableType.MASSENTITY // Entity to get velocity of
        };
        }
    }
    public override SpellVariableType ReturnType { get { return SpellVariableType.VECTOR2; } }

    public override SpellVariable Operate(SpellCaster spellCaster, params SpellVariable[] args)
    {
        IMassEntity entity = args[0].AsMassEntity();
        return new SpellVariable(SpellVariableType.VECTOR2, entity.massVelocity);
    }
}