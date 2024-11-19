
public class IntConstant : SelectorSpellPiece
{
	public int Value;
	public override SpellVariableType ReturnType { get { return SpellVariableType.INT; } }
	public override SpellVariableType[] ConfigList { get { return new SpellVariableType[] { SpellVariableType.INT }; } }

	public override void applyConfig(object[] configs)
	{
		Value = (int)configs[0];
	}
	public override object[] getConfigValues()
	{
		return new object[] { Value };
	}
	public IntConstant()
	{
	}
	public IntConstant(int value)
	{
		Value = value;
	}

	public override SpellVariable Select(SpellCaster spellCaster)
	{
		return new SpellVariable(SpellVariableType.INT, Value);
	}
}