using Godot;
using System;
using System.Runtime.CompilerServices;

public class MassAddMotion : ExecutorSpellPiece
{
    public override string Name { get { return "MassAddMotion"; } }
    public override SpellVariableType[] ParamList { get { return new SpellVariableType[] { SpellVariableType.LIVINGENTITY, SpellVariableType.VECTOR2 }; } }
    public override void Execute(SpellCaster spellCaster, params SpellVariable[] args)
    {
        checkParams(args);
        LivingEntity entity = args[0].AsEntity();
        Vector2 speed = args[1].AsVector2();
        if ((entity.speed == speed) || !spellCaster.TryToConsumeMana((int)speed.Length() * 5)) return;
        entity.speed = speed;
    }
}