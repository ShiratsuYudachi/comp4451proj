using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Linq;

public enum SpellVariableType
{
	NONE,
	INT,
	BOOL,
	FLOAT,
	LIVINGENTITY,
	MASSENTITY,
	//NODE2D,
	VECTOR2
	
}

public struct SpellVariable
{
	public SpellVariableType Type;
	private object _value;

	public SpellVariable(SpellVariableType type, object value)
	{
		if (value == null && type != SpellVariableType.NONE){
			throw new ArgumentException("Spell Variable Casting Error: casting null to " + type);
		}
		
		
		switch (type)
		{
			case SpellVariableType.NONE:
				if (!(value is null))
				{
					throw new ArgumentException("Spell Variable Casting Error: casting " + value.GetType() + " to null");
				}
				break;
			case SpellVariableType.INT:
				if (!(value is int))
				{
					throw new ArgumentException("Spell Variable Casting Error: casting " + value.GetType() + " to int");
				}
				break;
			case SpellVariableType.BOOL:
				if (!(value is bool))
				{
					throw new ArgumentException("Spell Variable Casting Error: casting " + value.GetType() + " to bool");
				}
				break;
			case SpellVariableType.FLOAT:
				if (!(value is float))
				{
					throw new ArgumentException("Spell Variable Casting Error: casting " + value.GetType() + " to float");
				}
				break;
			case SpellVariableType.LIVINGENTITY:
				if (!(value is LivingEntity))
				{
					throw new ArgumentException("Spell Variable Casting Error: casting " + value.GetType() + " to LivingEntity");
				}
				break;
			case SpellVariableType.MASSENTITY:
				if (!(value is IMassEntity))
				{
					throw new ArgumentException("Spell Variable Casting Error: casting " + value.GetType() + " to IMassEntity");
				}
				break;
			case SpellVariableType.VECTOR2:
				if (!(value is Vector2))
				{
					throw new ArgumentException("Spell Variable Casting Error: casting " + value.GetType() + " to Vector2");
				}
				break;
			// case SpellVariableType.NODE2D:
			// 	if (!(value is Node2D))
			// 	{
			// 		throw new ArgumentException("Spell Variable Casting Error: casting " + value.GetType() + " to Node2D");
			// 	}
			// 	break;
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
			throw new InvalidOperationException("Spell Variable Casting Error: casting " + Type + " to int");
		}
		return (int)_value;
	}

	public bool AsBool()
	{
		if (Type != SpellVariableType.BOOL)
		{
			throw new InvalidOperationException("Spell Variable Casting Error: casting " + Type + " to bool");
		}
		return (bool)_value;
	}

	public float AsFloat()
	{
		if (Type != SpellVariableType.FLOAT)
		{
			throw new InvalidOperationException("Spell Variable Casting Error: casting " + Type + " to float");
		}
		return (float)_value;
	}

	public LivingEntity AsEntity()
	{
		if (Type != SpellVariableType.LIVINGENTITY)
		{
			throw new InvalidOperationException("Spell Variable Casting Error: casting " + Type + " to LivingEntity");
		}
		return (LivingEntity)_value;
	}

	public Vector2 AsVector2()
	{
		if (Type != SpellVariableType.VECTOR2)
		{
			throw new InvalidOperationException("Spell Variable Casting Error: casting " + Type + " to Vector2");
		}
		return (Vector2)_value;
	}

	// public Node2D AsNode2D()
	// {
	// 	if (Type != SpellVariableType.NODE2D && Type != SpellVariableType.LIVINGENTITY)
	// 	{
	// 		throw new InvalidOperationException("Spell Variable Casting Error: casting " + Type + " to Node2D");
	// 	}
	// 	return (Node2D)_value;
	// }

	public IMassEntity AsMassEntity()
	{
		if (Type == SpellVariableType.MASSENTITY || Type == SpellVariableType.LIVINGENTITY )
		{
			return (IMassEntity)_value;
		}
		throw new InvalidOperationException("Spell Variable Casting Error: casting " + Type + " to IMassEntity");
	}

}




public abstract class SpellPiece
{
	public virtual string Name { get; }

	public virtual SpellVariableType[] ParamList { get; }

	public virtual SpellVariableType[] ConfigList { get { return new SpellVariableType[] { }; } }

	public virtual object[] getConfigValues() { return new object[] { }; } // for GUI loading config items
	public virtual void applyConfig(object[] configs) { } // for GUI saving config items
	public virtual SpellVariableType ReturnType { get; }


	public string[] configToString(object[] configValues)
	{
		if (configValues == null || configValues.Length == 0)
			return new string[0];

		var configTypes = ConfigList;
		if (configTypes.Length != configValues.Length)
			throw new ArgumentException("Config values length doesn't match ConfigList length");

		var result = new string[configValues.Length];
		
		for (int i = 0; i < configValues.Length; i++)
		{
			switch (configTypes[i])
			{
				case SpellVariableType.INT:
					result[i] = ((int)configValues[i]).ToString();
					break;
				case SpellVariableType.FLOAT:
					result[i] = ((float)configValues[i]).ToString("F6");
					break;
				case SpellVariableType.BOOL:
					result[i] = ((bool)configValues[i]).ToString();
					break;
				case SpellVariableType.VECTOR2:
					var vec = (Vector2)configValues[i];
					result[i] = $"{vec.X:F6},{vec.Y:F6}";
					break;
				default:
					throw new ArgumentException($"Unsupported config type: {configTypes[i]}");
			}
		}

		return result;
	}

	public object[] stringToConfig(string[] configStrings)
	{
		if (configStrings == null || configStrings.Length == 0)
			return new object[0];

		var configTypes = ConfigList;
		if (configStrings.Length != configTypes.Length)
			throw new ArgumentException("Config strings length doesn't match ConfigList length");

		var result = new object[configStrings.Length];
		
		for (int i = 0; i < configStrings.Length; i++)
		{
			switch (configTypes[i])
			{
				case SpellVariableType.INT:
					result[i] = int.Parse(configStrings[i]);
					break;
				case SpellVariableType.FLOAT:
					result[i] = float.Parse(configStrings[i]);
					break;
				case SpellVariableType.BOOL:
					result[i] = bool.Parse(configStrings[i]);
					break;
				case SpellVariableType.VECTOR2:
					var vecParts = configStrings[i].Split(',');
					if (vecParts.Length != 2)
						throw new ArgumentException("Invalid Vector2 format");
					result[i] = new Vector2(
						float.Parse(vecParts[0]),
						float.Parse(vecParts[1])
					);
					break;
				default:
					throw new ArgumentException($"Unsupported config type: {configTypes[i]}");
			}
		}

		return result;
	}


	// protected void checkParams(SpellVariable[] args)
	// {
	// 	if (ParamList.Length != args.Length)
	// 	{
	// 		throw new ArgumentException("SpellPiece: Invalid number of arguments");
	// 	}
	// 	for (int i = 0; i < args.Length; i++)
	// 	{
	// 		if (ParamList[i] != args[i].Type)
	// 		{
	// 			throw new ArgumentException($"SpellPiece: Invalid argument type at position {i}");
	// 		}
	// 	}
	// }
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
	public override SpellVariableType[] ParamList
	{
		get
		{
			return new SpellVariableType[] { };
		}
	}
	public abstract SpellVariable Select(SpellCaster spellCaster);
}

public class IntConstantSpellPiece : SelectorSpellPiece
{
	public int Value;
	public override SpellVariableType ReturnType { get { return SpellVariableType.INT; } }
	public override SpellVariableType[] ConfigList { get { return new SpellVariableType[] { SpellVariableType.INT }; } }
	public override string Name
	{
		get
		{
			return "Int: " + Value.ToString();
		}
	}
	public override void applyConfig(object[] configs)
	{
		Value = (int)configs[0];
	}
	public override object[] getConfigValues()
	{
		return new object[] { Value };
	}
	public IntConstantSpellPiece()
	{
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
public class FloatConstantSpellPiece : SelectorSpellPiece
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
	public FloatConstantSpellPiece()
	{
	}
	public FloatConstantSpellPiece(float value)
	{
		Value = value;
	}

	public override SpellVariable Select(SpellCaster spellCaster)
	{
		return new SpellVariable(SpellVariableType.FLOAT, Value);
	}
}

public class Vector2ConstantSpellPiece : SelectorSpellPiece
{
	public override string Name
	{
		get
		{
			return "Vector2: " + Value.ToString();
		}
	}
	public Vector2 Value;
	public override SpellVariableType ReturnType { get { return SpellVariableType.VECTOR2; } }
	public override SpellVariableType[] ConfigList { get { return new SpellVariableType[] { SpellVariableType.VECTOR2 }; } }
	

	public override void applyConfig(object[] configs)
	{
		Value = (Vector2)configs[0];
	}

	public override object[] getConfigValues()
	{
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

	public SpellVariable Evaluate(SpellCaster spellCaster)
	{
		SpellVariable[] castingParams = new SpellVariable[rootSpellPiece.ParamList.Length];

		if (rootSpellPiece is SelectorSpellPiece)
		{
			return ((SelectorSpellPiece)rootSpellPiece).Select(spellCaster);
		}

		for (int i = 0; i < castingParams.Length; i++)
		{
			castingParams[i] = childrenSpellPieces[i].Evaluate(spellCaster);
			if (castingParams[i].Type == SpellVariableType.NONE){ // when error occurs, return NULL type, and recursively cancel casting
				return new SpellVariable(SpellVariableType.NONE, null);
			}
		}

		if (rootSpellPiece is OperatorSpellPiece)
		{
			return ((OperatorSpellPiece)rootSpellPiece).Operate(spellCaster, castingParams);
		}

		if (rootSpellPiece is ExecutorSpellPiece)
		{
			((ExecutorSpellPiece)rootSpellPiece).Execute(spellCaster, castingParams);
		}
		return new SpellVariable(SpellVariableType.NONE, null);
	}

	public SpellEvaluationTreeNode(SpellPiece root)
	{
		this.rootSpellPiece = root;
		this.childrenSpellPieces = new SpellEvaluationTreeNode[root.ParamList.Length];
	}

	public void PrintTree(string indent = "")
	{
		GD.Print(indent + rootSpellPiece.Name);
		foreach (var child in childrenSpellPieces)
		{
			if (child != null)
				child.PrintTree(indent + "  ");
		}
	}


	
	public Godot.Collections.Dictionary ToJSON()
    {
        // 创建主对象
        var jsonObj = new Godot.Collections.Dictionary();
        
        // 添加拼写片段类型和名称
        jsonObj["Type"] = rootSpellPiece.GetType().Name;
        
        // 添加配置值（如果有）
        var configValues = rootSpellPiece.getConfigValues();

		jsonObj["config"] = rootSpellPiece.configToString(configValues);
		
        
        // 递归处理子节点
        if (childrenSpellPieces.Length > 0)
        {
            var childrenArray = new Godot.Collections.Array();
            foreach (var child in childrenSpellPieces)
            {
                childrenArray.Add(child != null ? child.ToJSON() : null);
            }
            jsonObj["children"] = childrenArray;
        }
		else{
			jsonObj["children"] = new Godot.Collections.Array();
		}
        
        return jsonObj;
    }

	public static SpellEvaluationTreeNode loadJSON(Godot.Collections.Dictionary jsonObj)
	{
		
		string typeName = (string)jsonObj["Type"];
		
		Type spellPieceType = Type.GetType(typeName);
		if (spellPieceType == null)
		{
			throw new ArgumentException($"Unknown spell piece type: {typeName}");
		}
		
		SpellPiece spellPiece = (SpellPiece)Activator.CreateInstance(spellPieceType);
		
		// 处理配置值
		var configArray = (Godot.Collections.Array)jsonObj["config"];
		if (configArray.Count > 0)
		{
			var configStrings = new string[configArray.Count];
			for (int i = 0; i < configArray.Count; i++)
			{
				configStrings[i] = (string)configArray[i];
			}
			spellPiece.applyConfig(spellPiece.stringToConfig(configStrings));
		}
		
		// 创建树节点
		var node = new SpellEvaluationTreeNode(spellPiece);
		
		// 递归处理子节点
		var childrenArray = (Godot.Collections.Array)jsonObj["children"];
		for (int i = 0; i < childrenArray.Count; i++)
		{
			var childData = (Godot.Collections.Dictionary)childrenArray[i];
			node.childrenSpellPieces[i] = loadJSON(childData);
		}
		
		return node;
	}

}



public class SpellStorage{
	public Dictionary<string, SpellEvaluationTreeNode> spells = new Dictionary<string, SpellEvaluationTreeNode>();

	public SpellStorage(){
		
	}

	public void AddSpell(string spellName, SpellEvaluationTreeNode spell){
		spells[spellName] = spell;
		SaveAllSpells();
	}	

	public string[] getSpellNames(){
		return spells.Keys.ToArray();
	}

	public SpellEvaluationTreeNode getSpell(string spellName){
		if (spellName == "None"){
			return null;
		}
		if (spells.ContainsKey(spellName)){
			return spells[spellName];
		}
		return null;
	}

	public void SaveAllSpells() {
		// Create directory if not exists
		string dirPath = System.IO.Path.Combine(
			System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),
			"SpellCompiler"
		);
		System.IO.Directory.CreateDirectory(dirPath);

		// Save each spell
		foreach (var spellPair in spells) {
			string jsonPath = System.IO.Path.Combine(dirPath, $"{spellPair.Key}.json");
			var jsonDict = spellPair.Value.ToJSON();
			string jsonString = Json.Stringify(jsonDict);
			System.IO.File.WriteAllText(jsonPath, jsonString);
		}
	}

	public void LoadAllSpells() {
		string dirPath = System.IO.Path.Combine(
			System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),
			"SpellCompiler"
		);

		if (!System.IO.Directory.Exists(dirPath)) return;

		spells.Clear();
		foreach (string file in System.IO.Directory.GetFiles(dirPath, "*.json")) {
			string spellName = System.IO.Path.GetFileNameWithoutExtension(file);
			string jsonString = System.IO.File.ReadAllText(file);
			var loadedDict = Json.ParseString(jsonString).AsGodotDictionary();
			spells[spellName] = SpellEvaluationTreeNode.loadJSON(loadedDict);
		}
	}

}
