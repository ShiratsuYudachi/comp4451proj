using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class SpellManager : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 inputVector = Input.GetVector("MoveLeft", "MoveRight", "MoveUp", "MoveDown");
	}
}

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
	public abstract void Execute(params SpellVariable[] args);
}

public abstract class OperatorSpellPiece : SpellPiece
{
	public abstract SpellVariable Operate(params SpellVariable[] args);
}

public abstract class SelectorSpellPiece : SpellPiece
{
	public override SpellVariableType[] ParamList { 
		get{
			return new SpellVariableType[] {};
		}
	}
	public abstract SpellVariable Select(Node2D spellExecutor);
}

public class IntConstantSpellPiece : SelectorSpellPiece
{
	public int Value;

	public override string Name {
		get{
			return "Int: " + Value.ToString();
		}
	}
	
	public IntConstantSpellPiece(int value)
	{
		Value = value;
	}

	public override SpellVariable Select(Node2D spellExecutor)
	{
		return new SpellVariable(SpellVariableType.INT, Value);
	}
}

public class Vector2ConstantSpellPiece : SelectorSpellPiece
{
	public Vector2 Value;

	public override string Name {
		get{
			return "Vector2: " + Value.ToString();
		}
	}

	public Vector2ConstantSpellPiece(Vector2 value)	
	{
		Value = value;
	}

	public override SpellVariable Select(Node2D spellExecutor)
	{
		return new SpellVariable(SpellVariableType.VECTOR2, Value);
	}
}


