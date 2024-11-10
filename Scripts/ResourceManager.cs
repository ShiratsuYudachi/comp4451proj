using System;
using Godot;
public enum ResourceType{
        Bullet,
        SpellPieceIcon,
    }
public class ResourceManager
{
    

    public static Texture2D GetTexture(ResourceType type, string name){
        switch (type){
            case ResourceType.Bullet:
                return GD.Load<Texture2D>("res://Art/Bullets/bullet.png");
            case ResourceType.SpellPieceIcon:
                Texture2D texture = GD.Load<Texture2D>("res://assets/Spells/SpellPieceIcons/"+name+".png");
                return texture;
            default:
                return null;
        }
    }
}