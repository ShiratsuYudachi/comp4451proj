using Godot;
using System;
using System.Runtime.CompilerServices;

public class VectorNormalize : OperatorSpellPiece
{


    public override SpellVariableType[] ParamList { get
    {
        return new SpellVariableType[] {
            SpellVariableType.Vector2 // Vector to normalize
        };
    }
    }
    public override SpellVariableType ReturnType { get { return SpellVariableType.Vector2; } }

    public override SpellVariable Operate(SpellCaster spellCaster, params SpellVariable[] args)
    {
        //checkParams(args);

        Vector2 vec = args[0].AsVector2();

        return new SpellVariable(SpellVariableType.Vector2, vec.Normalized());
    }	
}