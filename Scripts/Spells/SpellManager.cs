using Godot;
using System;
using System.Runtime.CompilerServices;

public enum SpellVariableType
{
	NONE,
	INT,
	BOOL,
	FLOAT,
	ENTITY,
	VECTOR2
}

public struct SpellVariable
{
	public SpellVariableType Type;
	private object _value;

	public SpellVariable(SpellVariableType type, object value)
	{
		switch (type)
		{
			case SpellVariableType.NONE:
				if (!(value is null))
				{
					throw new ArgumentException("Spell Variable Casting Error: casting "+value.GetType()+" to null");
				}
				break;
			case SpellVariableType.INT:
				if (!(value is int))
				{
					throw new ArgumentException("Spell Variable Casting Error: casting "+value.GetType()+" to int");
				}
				break;
			case SpellVariableType.BOOL:
				if (!(value is bool))
				{
						throw new ArgumentException("Spell Variable Casting Error: casting "+value.GetType()+" to bool");
				}
				break;
			case SpellVariableType.FLOAT:
				if (!(value is float))
				{
					throw new ArgumentException("Spell Variable Casting Error: casting "+value.GetType()+" to float");
				}
				break;
			case SpellVariableType.ENTITY:
				if (!(value is Node2D))
				{
					throw new ArgumentException("Spell Variable Casting Error: casting "+value.GetType()+" to Node2D");
				}
				break;
			case SpellVariableType.VECTOR2:
				if (!(value is Vector2))
				{
					throw new ArgumentException("Spell Variable Casting Error: casting "+value.GetType()+" to Vector2");
				}
				break;
			default:
				throw new ArgumentException("Invalid Spell Variable Type");
		}
		Type = type;
		_value = value;
	}

	public int AsInt()
	{
		if (Type != SpellVariableType.INT)
		{
			throw new InvalidOperationException("Spell Variable Casting Error: casting "+Type+" to int");
		}
		return (int)_value;
	}

	public bool AsBool()
	{
		if (Type != SpellVariableType.BOOL)
		{
			throw new InvalidOperationException("Spell Variable Casting Error: casting "+Type+" to bool");
		}
		return (bool)_value;
	}

	public float AsFloat()
	{
		if (Type != SpellVariableType.FLOAT)
		{
			throw new InvalidOperationException("Spell Variable Casting Error: casting "+Type+" to float");
		}
		return (float)_value;
	}

	public Node2D AsEntity()
	{
		if (Type != SpellVariableType.ENTITY)
		{
			throw new InvalidOperationException("Spell Variable Casting Error: casting "+Type+" to Node2D");
		}
		return (Node2D)_value;
	}

	public Vector2 AsVector2()
	{
		if (Type != SpellVariableType.VECTOR2)
		{
			throw new InvalidOperationException("Spell Variable Casting Error: casting "+Type+" to Vector2");
		}
		return (Vector2)_value;
	}
}




public abstract class SpellPiece
{
	public virtual string Name { get; }

	public virtual SpellVariableType[] ParamList { get; }

	public virtual SpellVariableType[] ConfigList { get { return new SpellVariableType[] {}; } }
	
	public virtual object[] getConfigValues(){return new object[] {};} // for GUI loading config items
	public virtual void applyConfig(object[] configs){} // for GUI saving config items
	public virtual SpellVariableType ReturnType { get; }


	protected void checkParams(SpellVariable[] args)
	{
		if (ParamList.Length != args.Length)
		{
			throw new ArgumentException("SpellPiece: Invalid number of arguments");
		}
		for (int i = 0; i < args.Length; i++)
		{
			if (ParamList[i] != args[i].Type)
			{
				throw new ArgumentException($"SpellPiece: Invalid argument type at position {i}");
			}
		}
	}
}

public abstract class ExecutorSpellPiece : SpellPiece
{
	public override SpellVariableType ReturnType { get { return SpellVariableType.NONE; } }
	public abstract void Execute(SpellCaster spellCaster, params SpellVariable[] args);
}

public abstract class OperatorSpellPiece : SpellPiece
{
	public abstract SpellVariable Operate(SpellCaster spellCaster, params SpellVariable[] args);
}

public abstract class SelectorSpellPiece : SpellPiece
{
	public override SpellVariableType[] ParamList { 
		get{
			return new SpellVariableType[] {};
		}
	}
	public abstract SpellVariable Select(SpellCaster spellCaster);
}

public class IntConstantSpellPiece : SelectorSpellPiece
{
	public int Value;
	public override SpellVariableType ReturnType { get { return SpellVariableType.INT; } }
	public override string Name {
		get{
			return "Int: " + Value.ToString();
		}
	}
	
	public IntConstantSpellPiece(int value)
	{
		Value = value;
	}

	public override SpellVariable Select(SpellCaster spellCaster)
	{
		return new SpellVariable(SpellVariableType.INT, Value);
	}
}

public class Vector2ConstantSpellPiece : SelectorSpellPiece
{
	public Vector2 Value;
	public override SpellVariableType ReturnType { get { return SpellVariableType.VECTOR2; } }
	public override SpellVariableType[] ConfigList { get { return new SpellVariableType[] { SpellVariableType.VECTOR2 }; } }
	public override string Name {
		get{
			return "Vector2: " + Value.ToString();
		}
	}

	public override void applyConfig(object[] configs){
		Value = (Vector2)configs[0];
	}

	public override object[] getConfigValues(){
		return new object[] { Value };
	}

	public Vector2ConstantSpellPiece()	
	{
	}

	public Vector2ConstantSpellPiece(Vector2 value)	
	{
		Value = value;
	}

	public override SpellVariable Select(SpellCaster spellCaster)
	{
		return new SpellVariable(SpellVariableType.VECTOR2, Value);
	}
}
// Start of Selection

public class SpellEvaluationTreeNode
{
	public SpellPiece rootSpellPiece;

	public SpellEvaluationTreeNode[] childrenSpellPieces;

	public SpellVariable Evaluate(SpellCaster spellCaster){
		SpellVariable[] castingParams = new SpellVariable[rootSpellPiece.ParamList.Length];
		
		if (rootSpellPiece is SelectorSpellPiece){
			return ((SelectorSpellPiece)rootSpellPiece).Select(spellCaster);
		}
		
		for (int i = 0; i < castingParams.Length; i++)
		{
			castingParams[i] = childrenSpellPieces[i].Evaluate(spellCaster);
		}
		
		if (rootSpellPiece is OperatorSpellPiece){
			return ((OperatorSpellPiece)rootSpellPiece).Operate(spellCaster, castingParams);
		}

		if (rootSpellPiece is ExecutorSpellPiece){
			((ExecutorSpellPiece)rootSpellPiece).Execute(spellCaster, castingParams);
		}
		return new SpellVariable(SpellVariableType.NONE, null);
	}

	public SpellEvaluationTreeNode(SpellPiece root){
		this.rootSpellPiece = root;
		this.childrenSpellPieces = new SpellEvaluationTreeNode[root.ParamList.Length];
	}

	public void PrintTree(string indent = "")
	{
		GD.Print(indent + rootSpellPiece.Name);
		foreach(var child in childrenSpellPieces)
		{
			if(child != null)
				child.PrintTree(indent + "  ");
		}
	}
}
