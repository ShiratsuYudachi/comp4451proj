using Godot;
using System;

public class Heal : ExecutorSpellPiece
{
    public override string Name { get { return "Heal"; } }
    public override SpellVariableType[] ParamList { get { return new SpellVariableType[] { SpellVariableType.LIVINGENTITY, SpellVariableType.INT }; } }
    public override void Execute(SpellCaster spellCaster, params SpellVariable[] args)
    {
        checkParams(args);
        LivingEntity target = args[0].AsEntity();
        int deltaMP = args[1].AsInt();
        if (spellCaster.TryToConsumeMana(deltaMP))
        {
            int deltaHP = (int)(deltaMP * 2.61);
            target.health = target.health + deltaHP > target.MAX_HEALTH ? target.MAX_HEALTH : target.health + deltaHP;
        }
    }
}