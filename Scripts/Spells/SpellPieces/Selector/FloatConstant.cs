public class FloatConstant : SelectorSpellPiece
{
	public float Value;
	public override SpellVariableType ReturnType { get { return SpellVariableType.FLOAT; } }
	public override SpellVariableType[] ConfigList { get { return new SpellVariableType[] { SpellVariableType.FLOAT }; } }
	public override string Name
	{
		get
		{
			return "Float: " + Value.ToString();
		}
	}
	public override void applyConfig(object[] configs)
	{
		Value = (float)configs[0];
	}
	public override object[] getConfigValues()
	{
		return new object[] { Value };
	}
	public FloatConstant()
	{
	}
	public FloatConstant(float value)
	{
		Value = value;
	}

	public override SpellVariable Select(SpellCaster spellCaster)
	{
		return new SpellVariable(SpellVariableType.FLOAT, Value);
	}
}
