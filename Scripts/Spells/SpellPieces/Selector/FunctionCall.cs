using Godot;
using System;


public class FunctionCall : SelectorSpellPiece
{
	public int Value;
	public override SpellVariableType ReturnType { get { return SpellVariableType.Vector2; } }
	

	public override SpellVariable Select(SpellCaster spellCaster)
	{
		return new SpellVariable(SpellVariableType.Vector2, GameScene.instance.GetGlobalMousePosition() - spellCaster.GlobalPosition);
	}
}