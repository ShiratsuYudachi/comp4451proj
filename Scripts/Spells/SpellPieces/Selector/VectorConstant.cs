using Godot;
public class Vector2Constant : SelectorSpellPiece
{
	public Vector2 Value;
	public override SpellVariableType ReturnType { get { return SpellVariableType.Vector2; } }
	public override SpellVariableType[] ConfigList { get { return new SpellVariableType[] { SpellVariableType.Vector2 }; } }
	

	public override void applyConfig(object[] configs)
	{
		Value = (Vector2)configs[0];
	}

	public override object[] getConfigValues()
	{
		return new object[] { Value };
	}

	public Vector2Constant()
	{
	}

	public Vector2Constant(Vector2 value)
	{
		Value = value;
	}

	public override SpellVariable Select(SpellCaster spellCaster)
	{
		return new SpellVariable(SpellVariableType.Vector2, Value);
	}
}