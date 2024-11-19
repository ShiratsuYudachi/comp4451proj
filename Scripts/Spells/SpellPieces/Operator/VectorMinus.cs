using Godot;
using System;
using System.Runtime.CompilerServices;

public class VectorMinus : OperatorSpellPiece
{
	public override string Name
    {
        get 
        { 
            return "Vector Minus"; 
        }
    }

    public override SpellVariableType[] ParamList { get
    {
        return new SpellVariableType[] {
            SpellVariableType.Vector2, // Vector to subtract
            SpellVariableType.Vector2 // Vector to subtract
        };
    }
    }
    public override SpellVariableType ReturnType { get { return SpellVariableType.Vector2; } }

    public override SpellVariable Operate(SpellCaster spellCaster, params SpellVariable[] args)
    {
        //checkParams(args);

        Vector2 vec1 = args[0].AsVector2();
        Vector2 vec2 = args[1].AsVector2();

        return new SpellVariable(SpellVariableType.Vector2, vec1 - vec2);
    }	
}