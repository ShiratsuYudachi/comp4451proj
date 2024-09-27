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

	public override SpellVariableType[] ParamList { get
    {
        return new SpellVariableType[] {
            SpellVariableType.ENTITY,
            SpellVariableType.VECTOR2
        };
    }
    }

    public override void Execute(params SpellVariable[] args)
    {
        checkParams(args);
        
        Node2D entity = args[0].AsEntity();
        Vector2? targetRelativePos = args[1].AsVector2();
        entity.Position += targetRelativePos.Value;
    }	
}