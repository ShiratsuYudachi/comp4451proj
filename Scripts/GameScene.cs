using Godot;
using System;


public partial class GameScene : Node2D
{
    public static GameScene instance;
    public static PackedScene damageLabelScene;
    public static Camera2D mainCamera;
    public static PlayerControl player;
    public static SpellStorage playerSpellStorage = new SpellStorage();

    public static CanvasLayer UI;

    public override void _Ready()
    {
        if (player == null)
        {
            player = GD.Load<PackedScene>("res://Scenes/LivingEntities/Player.tscn").Instantiate<PlayerControl>();
            this.AddChild(player);
            GD.Print("[INFO] GameScene: Player created!"+player.Name);
        }
        if (UI == null)
        {
            UI = GetNode<CanvasLayer>("CanvasLayer");
        }

        if (player != null){
            mainCamera = player.GetNode<Camera2D>("Camera2D");
        }
        
        damageLabelScene = GD.Load<PackedScene>("res://Scenes/UI/damageTipLabel.tscn");
        instance = this;
        GD.Print("GameScene instance created: " + instance.Name);

    }

    public static void ShowTip(string tip){
        TipManager tipManager = UI.GetNode<TipManager>("Ui/TipManager");
        tipManager.ShowTip(tip);
    }

    public static void ShowDamage(float damage, Vector2 worldPosition)
    {
        //Control damageLabelParent = damageLabelScene.Instantiate<Control>();
        Label damageLabel = damageLabelScene.Instantiate<Label>();
        damageLabel.Text = ((int)damage).ToString();
        damageLabel.GlobalPosition = worldPosition + new Vector2(5, 3);
        instance.AddChild(damageLabel);
    }

    public static void ShowSpellAnimation(Vector2 worldPosition){
        PackedScene spellAnimationScene = GD.Load<PackedScene>("res://Scenes/SpellCastingCircle.tscn");
        Node2D spellAnimation = spellAnimationScene.Instantiate<Node2D>();
        instance.AddChild(spellAnimation);
        spellAnimation.GlobalPosition = worldPosition;
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

    public static void SpawnEntity(String entityName, Vector2 position){
        PackedScene entityScene = GD.Load<PackedScene>("res://Scenes/LivingEntities/" + entityName + ".tscn");
        if (entityScene == null)
        {
            GD.PrintErr("Entity scene not found: " + entityName);
            return;
        }
        LivingEntity entity = entityScene.Instantiate<LivingEntity>();
        instance.AddChild(entity);
        entity.GlobalPosition = position;
    }

    public static void inGameLog(string message){
        ShowTip(message); 
    }

    public static Bullet CreateBullet(Vector2 position){
        PackedScene bulletScene = GD.Load<PackedScene>("res://Scenes/Bullet/Bullet.tscn");
        Bullet bullet = bulletScene.Instantiate<Bullet>();
        instance.AddChild(bullet);
        bullet.GlobalPosition = position;
        return bullet;
    }
}

