using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Godot;

public struct SpellPieceInfo{
    public string name;
    public string description;
    public Type spellClassType;

    public List<string> configNames;
    public List<string> paramNames;

    public SpellPieceInfo(string name, string description, Type spellClassType, List<string> configNames, List<string> paramNames)
    {
        this.name = name;
        this.description = description;
        this.spellClassType = spellClassType;
        this.configNames = configNames;
        this.paramNames = paramNames;
    }

    public string getConfigName(int index){
        if (configNames == null || configNames.Count <= index)
            return "";
        return configNames[index];
    }

    public string getParamName(int index, string defaultName){
        if (paramNames == null || paramNames.Count <= index)
            return defaultName;
        return paramNames[index];
    }
}

public static class SpellRegistry{
    private static List<SpellPieceInfo> spellPieces = new List<SpellPieceInfo>();

    public static void RegisterSpellPiece(string name, string description, Type spellClassType, List<string> configNames = null, List<string> paramNames = null )
    {
        spellPieces.Add(new SpellPieceInfo(name, description, spellClassType, configNames, paramNames));
    }

    

    public static void Initialize()
    {

        // Executor
        RegisterSpellPiece(
            "GenerateElementalOrb",
            "Generates a elemental orb. Element Index Pyro(0), Electro(1), Hydro(2), Dendro(3), Cryo(4)",
            typeof(GenerateElementalOrb),
            configNames: new List<string>{"ElementIndex"},
            paramNames: new List<string>{"Position", "Velocity"}
            
        );
        RegisterSpellPiece(
            "Blink",
            "Teleports the caster by the displacement vector",
            typeof(Blink),
            paramNames: new List<string>{"EntityToBlink", "Displacement"}
        );
        RegisterSpellPiece(
            "Heal",
            "Heals the target entity",
            typeof(Heal),
            paramNames: new List<string>{"Target", "HealAmount"}
        );
        RegisterSpellPiece(
            "MassAddMotion", 
            "Adds motion to a mass entity",
            typeof(MassAddMotion),
            paramNames: new List<string>{"Target", "Motion"}
        );
        // Operator
        RegisterSpellPiece(
            "GetEntityPos",
            "Gets the position of an entity",
            typeof(GetEntityPos)
        );
        RegisterSpellPiece(
            "GetEntityVelocity", 
            "Gets the velocity of an entity",
            typeof(GetEntityVelocity)
        );
        RegisterSpellPiece(
            "VectorMinus",
            "Subtracts two vectors",
            typeof(VectorMinus)
        );
        RegisterSpellPiece(
            "VectorMultiplication",
            "Multiplies a vector by a scalar",
            typeof(VectorMultiplication)
        );
        RegisterSpellPiece(
            "VectorNormalize",
            "Normalizes a vector",
            typeof(VectorNormalize)
        );

        // Selector
        RegisterSpellPiece(
            "SelectMousePos",
            "Selects the mouse position",
            typeof(SelectMousePos)
        );
        RegisterSpellPiece(
            "SelectCaster",
            "Selects the caster",
            typeof(SelectCaster)
        );
        RegisterSpellPiece(
            "SelectNearestBullet",
            "Selects the nearest bullet to the caster",
            typeof(SelectNearestBullet),
            configNames: new List<string>{"AvoidDuplicate"}
        );
        // Constants
        RegisterSpellPiece(
            "Vector2Constant",
            "A constant vector value",
            typeof(Vector2Constant)
        );
        RegisterSpellPiece(
            "IntConstant",
            "A constant integer value", 
            typeof(IntConstant)
        );
        RegisterSpellPiece(
            "FloatConstant",
            "A constant float value",
            typeof(FloatConstant)
        );


        // FunctionOut
        RegisterSpellPiece(
            "FunctionOut",
            "Create a function spell. ReturnTypeIndex: None(0), Int(1), Bool(2), Float(3), LivingEntity(4), MassEntity(5), Vector2(6)",
            typeof(FunctionOut),
            configNames: new List<string>{"ReturnTypeIndex"}
        );


    }

    public static ReadOnlyCollection<SpellPieceInfo> GetAllSpellPieces(){
        return spellPieces.AsReadOnly();
    }

    public static SpellPieceInfo GetSpellPieceInfo(string name){
        return spellPieces.Find(info => info.name == name);
    }

    public static SpellPieceInfo GetSpellPieceInfo(int index){
        return spellPieces[index];
    }
}
