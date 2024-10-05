using Godot;
using System;
using System.Runtime.CompilerServices;

public class GetEntityPos : OperatorSpellPiece
{
    public override string Name
    {
        get
        {
            return "Get Entity Position";
        }
    }

    public override SpellVariableType[] ParamList
    {
        get
        {
            return new SpellVariableType[] {
            SpellVariableType.LIVINGENTITY // Entity to get position of
        };
        }
    }
    public override SpellVariableType ReturnType { get { return SpellVariableType.VECTOR2; } }

    public override SpellVariable Operate(SpellCaster spellCaster, params SpellVariable[] args)
    {
        checkParams(args);
        LivingEntity entity = args[0].AsEntity();
        return new SpellVariable(SpellVariableType.VECTOR2, entity.Position);
    }
}