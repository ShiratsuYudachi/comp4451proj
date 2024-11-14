using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Godot;

public struct SpellPieceInfo{
    public string name;
    public string description;
    public Type spellClassType;

    public SpellPieceInfo(string name, string description, Type spellClassType)
    {
        this.name = name;
        this.description = description;
        this.spellClassType = spellClassType;
    }
}

public static class SpellRegistry{
    private static List<SpellPieceInfo> spellPieces = new List<SpellPieceInfo>();

    public static void RegisterSpellPiece(string name, string description, Type spellClassType)
    {
        spellPieces.Add(new SpellPieceInfo(name, description, spellClassType));
    }

    public static void Initialize()
    {

        // Executor
        RegisterSpellPiece(
            "GenerateElementalOrb",
            "Generates a elemental orb",
            typeof(GenerateElementalOrb)
        );
        RegisterSpellPiece(
            "Blink",
            "Teleports the caster to the mouse position",
            typeof(Blink)
        );
        RegisterSpellPiece(
            "Heal",
            "Heals the target entity",
            typeof(Heal)
        );
        RegisterSpellPiece(
            "MassAddMotion", 
            "Adds motion to a mass entity",
            typeof(MassAddMotion)
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
            typeof(SelectNearestBullet)
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
    }

    public static ReadOnlyCollection<SpellPieceInfo> GetAllSpellPieces(){
        GD.Print("SpellRegistry: GetAllSpellPieces: " + spellPieces.Count);
        return spellPieces.AsReadOnly();
    }

    public static SpellPieceInfo GetSpellPieceInfo(string name){
        return spellPieces.Find(info => info.name == name);
    }

    public static SpellPieceInfo GetSpellPieceInfo(int index){
        return spellPieces[index];
    }
}
