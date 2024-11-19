using Godot;
using System;
using System.Runtime.CompilerServices;

public class VectorMultiplication : OperatorSpellPiece
{
	public override string Name
    {
        get 
        { 
            return "Vector Multiplication"; 
        }
    }

    public override SpellVariableType[] ParamList { get
    {
        return new SpellVariableType[] {
            SpellVariableType.Vector2, // Vector to multiply
            SpellVariableType.FLOAT // Scalar to multiply
        };
    }
    }
    public override SpellVariableType ReturnType { get { return SpellVariableType.Vector2; } }

    public override SpellVariable Operate(SpellCaster spellCaster, params SpellVariable[] args)
    {
        //checkParams(args);

        Vector2 vec1 = args[0].AsVector2();
        float scalar = args[1].AsFloat();

        return new SpellVariable(SpellVariableType.Vector2, vec1 * scalar);
    }	
}