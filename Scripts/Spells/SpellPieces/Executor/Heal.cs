using Godot;
using System;

public class Heal : ExecutorSpellPiece
{
    public override SpellVariableType[] ParamList { 
        get {
            return new SpellVariableType[] {
            SpellVariableType.LivingEntity, // Target to heal
            SpellVariableType.INT // Amount of mana to consume
        };
        }
    }
    public override void Execute(SpellCaster spellCaster, params SpellVariable[] args)
    {
        //checkParams(args);
        LivingEntity target = args[0].AsEntity();
        int deltaMP = args[1].AsInt();
        if (spellCaster.TryToConsumeMana(deltaMP))
        {
            int deltaHP = (int)(deltaMP * 2.61);
            target.Health = target.Health + deltaHP > target.MaxHealth ? target.MaxHealth : target.Health + deltaHP;
        }
    }
}