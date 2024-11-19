using System;
using Godot;
using Chemistry;


public enum TextureResourceType{
Bullet,
SpellPieceIcon,
ElementIcon,
}

public enum SceneResourceType{
    ElementDisplay,
    Player,
    DamageLabel,
    SpellCastingCircle,
    Bullet,
    ElementalOrb,
    LivingEntity,
    AOE_Trigger,
    ExplosionEffect,
    ReactionTipLabel,
}

public class ResourceManager
{
    private static readonly string LIVING_ENTITIES_PATH = "res://Scenes/LivingEntities/";
    private static readonly string BULLET_PATH = "res://Scenes/Bullet/";
    private static readonly string UI_PATH = "res://Scenes/UI/";
    private static readonly string PARTICLE_PATH = "res://Scenes/Particle/";
    

    public static Texture2D GetTexture(TextureResourceType type, string name){
        switch (type){
            case TextureResourceType.Bullet:
                return GD.Load<Texture2D>("res://Art/Bullets/bullet.png");
            
            case TextureResourceType.SpellPieceIcon:
                return GD.Load<Texture2D>("res://assets/Spells/SpellPieceIcons/"+name+".png");
            
            case TextureResourceType.ElementIcon:
                return GD.Load<Texture2D>("res://assets/UI/Elements/"+name+".png");
            default:
                return null;
        }
    }

    public static PackedScene GetScene(SceneResourceType type, string name = "")
    {
        switch (type)
        {
            case SceneResourceType.ElementDisplay:
                return GD.Load<PackedScene>(UI_PATH + "Chemistry/element_display.tscn");
            case SceneResourceType.Player:
                return GD.Load<PackedScene>(LIVING_ENTITIES_PATH + "Player.tscn");
            case SceneResourceType.DamageLabel:
                return GD.Load<PackedScene>(UI_PATH + "damageTipLabel.tscn");
            case SceneResourceType.SpellCastingCircle:
                return GD.Load<PackedScene>("res://Scenes/SpellCastingCircle.tscn");
            case SceneResourceType.Bullet:
                return GD.Load<PackedScene>(BULLET_PATH + "Bullet.tscn");
            case SceneResourceType.ElementalOrb:
                return GD.Load<PackedScene>(BULLET_PATH + "ElementalOrb.tscn");
            case SceneResourceType.LivingEntity:
                return GD.Load<PackedScene>(LIVING_ENTITIES_PATH + name + ".tscn");
            case SceneResourceType.AOE_Trigger:
                return GD.Load<PackedScene>("res://Scenes/Utils/aoe_trigger.tscn");
            case SceneResourceType.ExplosionEffect:
                return GD.Load<PackedScene>("res://Scenes/Particle/Effects/Explosion.tscn");
            case SceneResourceType.ReactionTipLabel:
                return GD.Load<PackedScene>("res://Scenes/UI/reactionTipLabel.tscn");
            default:
                return null;
        }
    }

    public static PackedScene GetReactionParticle(Reaction reaction){
        switch (reaction){
            case Reaction.Burning:
                return GD.Load<PackedScene>("res://Scenes/Particle/Reaction/BurningReaction.tscn");
            case Reaction.Vaporize:
                return GD.Load<PackedScene>("res://Scenes/Particle/Reaction/VaporizeReaction.tscn");
            case Reaction.Freeze:
                return GD.Load<PackedScene>("res://Scenes/Particle/Reaction/FreezeReaction.tscn");
            case Reaction.Melt:
                return GD.Load<PackedScene>("res://Scenes/Particle/Reaction/MeltReaction.tscn");
            case Reaction.Overloaded:
                return GetScene(SceneResourceType.ExplosionEffect);
            // case Reaction.Superconduct:
            //     return GD.Load<PackedScene>("res://Scenes/Particle/Reaction/SuperconductReaction.tscn");
            case Reaction.ElectroCharged:
                return GD.Load<PackedScene>("res://Scenes/Particle/Reaction/ElectroChargedReaction.tscn");
            default:
                return null;
        }
    }
}
