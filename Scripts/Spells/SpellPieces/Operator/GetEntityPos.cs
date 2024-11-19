using Godot;
using System;
using System.Runtime.CompilerServices;

public class GetEntityPos : OperatorSpellPiece
{


    public override SpellVariableType[] ParamList
    {
        get
        {
            return new SpellVariableType[] {
            SpellVariableType.MassEntity // Entity to get position of
        };
        }
    }
    public override SpellVariableType ReturnType { get { return SpellVariableType.Vector2; } }

    public override SpellVariable Operate(SpellCaster spellCaster, params SpellVariable[] args)
    {
        IMassEntity entity = args[0].AsMassEntity();
        return new SpellVariable(SpellVariableType.Vector2, entity.massPosition);
    }
}