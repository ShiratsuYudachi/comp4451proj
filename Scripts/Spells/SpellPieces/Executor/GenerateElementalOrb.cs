using Godot;
using System;
using System.Runtime.CompilerServices;

public class GenerateElementalOrb : ExecutorSpellPiece
{


    public override SpellVariableType[] ParamList
    {
        get
        {
            return new SpellVariableType[] {
            SpellVariableType.Vector2, // Bullet position
            SpellVariableType.Vector2 // Bullet velocity
        };
        }
    }


    // configs

    public override SpellVariableType[] ConfigList { get { return new SpellVariableType[] { SpellVariableType.INT }; } }

    public int elementIndex;

    public override void applyConfig(object[] configs)
	{
		elementIndex = (int)configs[0];
	}

	public override object[] getConfigValues()
	{
		return new object[] { elementIndex };
	}



    public override SpellVariableType ReturnType { get { return SpellVariableType.MassEntity; } }

    public override void Execute(SpellCaster spellCaster, params SpellVariable[] args)
    {
        //checkParams(args);
        if (!spellCaster.TryToConsumeMana(500)){
            return;
        }
        Vector2 bulletPos = args[0].AsVector2();
        GameScene.ShowSpellAnimation(bulletPos);
        ElementalOrb elementalOrb = GameScene.CreateElementalOrb(bulletPos, (Chemistry.Element)elementIndex);
        elementalOrb.caster = spellCaster.GetParent<Entity>();
        if (!spellCaster.TryToConsumeMana((int)(args[1].AsVector2().Length() * 5))){
            return;
        }
        elementalOrb.velocity = args[1].AsVector2();
    }
}