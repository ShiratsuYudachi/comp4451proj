using Godot;
using System;
using System.Runtime.CompilerServices;

public class FunctionOut : ExecutorSpellPiece
{


    public override SpellVariableType[] ParamList
    {
        get
        {
            return new SpellVariableType[] {
            getReturnType() // Entity to get position of
        };
        }
    }

    public override SpellVariableType[] ConfigList { get { return new SpellVariableType[] { SpellVariableType.INT }; } }


    private int returnTypeIndex = 0;
	public override void applyConfig(object[] configs)
	{
		returnTypeIndex = (int)configs[0];
	}
	public override object[] getConfigValues()
	{
		return new object[] { returnTypeIndex };
	}

    private SpellVariableType getReturnType(){
        return (SpellVariableType)returnTypeIndex;
    }

    public override void Execute(SpellCaster spellCaster, params SpellVariable[] args)
    {
        
    }
}