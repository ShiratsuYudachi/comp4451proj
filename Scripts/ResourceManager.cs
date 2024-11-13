using System;
using Godot;
public enum ResourceType{
        Bullet,
        SpellPieceIcon,
        ElementIcon,
    }
public class ResourceManager
{
    

    public static Texture2D GetTexture(ResourceType type, string name){
        switch (type){
            case ResourceType.Bullet:
                return GD.Load<Texture2D>("res://Art/Bullets/bullet.png");
            
            case ResourceType.SpellPieceIcon:
                return GD.Load<Texture2D>("res://assets/Spells/SpellPieceIcons/"+name+".png");
            
            case ResourceType.ElementIcon:
                return GD.Load<Texture2D>("res://assets/UI/Elements/"+name+".png");
            default:
                return null;
        }
    }
}