using Godot;
using System;


public partial class GameScene : Node2D
{
    public static GameScene instance;
    public static PackedScene damageLabelScene;
    public static Camera2D mainCamera;

    public override void _Ready()
    {
        mainCamera = GetTree().CurrentScene.GetViewport().GetCamera2D();
        damageLabelScene = GD.Load<PackedScene>("res://Scenes/UI/damageTipLabel.tscn");
        instance = this;
        GD.Print("GameScene instance created: " + instance.Name);
    }

    public static void ShowDamage(float damage, Vector2 worldPosition)
    {
        //Control damageLabelParent = damageLabelScene.Instantiate<Control>();
        Label damageLabel = damageLabelScene.Instantiate<Label>();
        damageLabel.Text = damage.ToString();
        damageLabel.GlobalPosition = worldPosition;
        instance.AddChild(damageLabel);
    }

    public static Vector2 WorldToScreenPosition(Vector2 worldPosition)
    {
        if (mainCamera == null)
        {
            GD.PrintErr("Main camera not set");
            return Vector2.Zero;
        }

        Vector2 viewportSize = mainCamera.GetViewport().GetVisibleRect().Size;
        Vector2 cameraOffset = mainCamera.GlobalPosition - (viewportSize / 2);
        Vector2 screenPosition = worldPosition - cameraOffset;

        return screenPosition;
    }
}

