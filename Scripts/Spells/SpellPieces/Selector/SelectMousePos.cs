using Godot;
using System;
using System.Runtime.CompilerServices;

public class SelectMousePos : SelectorSpellPiece
{

    public override SpellVariableType ReturnType { get { return SpellVariableType.Vector2; } }


    public override SpellVariable Select(SpellCaster spellCaster)
    {
        Vector2 mousePos = spellCaster.GetGlobalMousePosition();
        return new SpellVariable(SpellVariableType.Vector2, mousePos);
    }	
}