using Godot;
using System;
using System.Runtime.CompilerServices;

public class SelectCaster : SelectorSpellPiece
{
	public override string Name
    {
        get 
        { 
            return "Select Caster"; 
        }
    }


    public override SpellVariable Select(Node2D spellExecutor)
    {
        Node2D caster = spellExecutor.GetParent<Node2D>();
        return new SpellVariable(SpellVariableType.ENTITY, caster);
    }	
}