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
}

public class ResourceManager
{
    

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

    public static PackedScene GetScene(SceneResourceType type){
        switch (type){
            case SceneResourceType.ElementDisplay:
                return GD.Load<PackedScene>("res://Scenes/UI/Chemistry/element_display.tscn");
            default:
                return null;
        }
    }

    public static PackedScene GetReactionParticle(Reaction reaction){
        switch (reaction){
            case Reaction.Burning:
                return GD.Load<PackedScene>("res://Scenes/Particle/Reaction/BurningReaction.tscn");
            default:
                return null;
        }
    }
}
