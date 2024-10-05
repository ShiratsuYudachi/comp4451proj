using Godot;
using System;
using System.Runtime.CompilerServices;

public class SelectMousePos : SelectorSpellPiece
{
	public override string Name
    {
        get 
        { 
            return "Select Mouse Position"; 
        }
    }
    public override SpellVariableType ReturnType { get { return SpellVariableType.VECTOR2; } }


    public override SpellVariable Select(SpellCaster spellCaster)
    {
        Vector2 mousePos = spellCaster.GetGlobalMousePosition();
        return new SpellVariable(SpellVariableType.VECTOR2, mousePos);
    }	
}