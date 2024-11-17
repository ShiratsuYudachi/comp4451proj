using Godot;
using System;
using Chemistry;
using System.Collections.Generic;


public partial class GameScene : Node2D
{
    public static GameScene instance;
    public static PackedScene damageLabelScene;
    public static Camera2D mainCamera;
    public static PlayerControl player;
    public static SpellStorage playerSpellStorage = new SpellStorage();

    public static CanvasLayer UI;

    bool gameInitialized = false;

    private static Queue<(Vector2 position, int radius, Action<Entity> onTrigger)> pendingAOEs = 
        new Queue<(Vector2, int, Action<Entity>)>();

    public override void _EnterTree()
    {
        if (!gameInitialized){
            Initialize();
            gameInitialized = true;
        }
    }

    public override void _Ready()
    {
        if (player == null)
        {
            var playerScene = ResourceManager.GetScene(SceneResourceType.Player);
            player = playerScene.Instantiate<PlayerControl>();
            this.AddChild(player);
            GD.Print("[INFO] GameScene: Player created!" + player.Name);
        }
        if (UI == null)
        {
            UI = GetNode<CanvasLayer>("CanvasLayer");
        }

        if (player != null)
        {
            mainCamera = player.GetNode<Camera2D>("Camera2D");
        }

        damageLabelScene = ResourceManager.GetScene(SceneResourceType.DamageLabel);
        instance = this;
        GD.Print("GameScene instance created: " + instance.Name);
    }


    // called only once when the game starts
    public static void Initialize(){
        SpellRegistry.Initialize();
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

    public static void ShowReaction(Reaction reaction, Vector2 worldPosition)
    {
        //Control damageLabelParent = damageLabelScene.Instantiate<Control>();\
        string reactionString = reaction.ToString();

        switch (reaction){
            case Reaction.Burning:
                reactionString = "燃烧";
                break;
            case Reaction.Vaporize:
                reactionString = "蒸发";
                break;
            case Reaction.Superconduct:
                reactionString = "超导";
                break;
            case Reaction.Freeze:
                reactionString = "冻结";
                break;
            case Reaction.Overloaded:
                reactionString = "超载";
                break;
        }
        Label damageLabel = damageLabelScene.Instantiate<Label>();
        damageLabel.Text = reactionString;
        damageLabel.GlobalPosition = worldPosition + new Vector2(5, 3);
        instance.AddChild(damageLabel);
        PackedScene particleScene = ResourceManager.GetReactionParticle(reaction);
        if (particleScene == null){
            return;
        }
        OneTimeEffect particle = particleScene.Instantiate<OneTimeEffect>();
        instance.AddChild(particle);
        particle.GlobalPosition = worldPosition;
    }

    public static void ShowSpellAnimation(Vector2 worldPosition)
    {
        PackedScene spellAnimationScene = ResourceManager.GetScene(SceneResourceType.SpellCastingCircle);
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

    public static void SpawnEntity(String entityName, Vector2 position)
    {
        PackedScene entityScene = ResourceManager.GetScene(SceneResourceType.LivingEntity, entityName);
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

    public static Bullet CreateBullet(Vector2 position)
    {
        PackedScene bulletScene = ResourceManager.GetScene(SceneResourceType.Bullet);
        Bullet bullet = bulletScene.Instantiate<Bullet>();
        instance.AddChild(bullet);
        bullet.GlobalPosition = position;
        return bullet;
    }

    public static ElementalOrb CreateElementalOrb(Vector2 position, Element element)
    {
        PackedScene elementalOrbScene = ResourceManager.GetScene(SceneResourceType.ElementalOrb);
        ElementalOrb elementalOrb = elementalOrbScene.Instantiate<ElementalOrb>();
        instance.AddChild(elementalOrb);
        elementalOrb.GlobalPosition = position;
        elementalOrb.SetElement(element);
        return elementalOrb;
    }

    public static void CreateAOE_Trigger(Vector2 position, int radius, Action<Entity> onTrigger)
    {
        // 将AOE创建请求加入队列
        pendingAOEs.Enqueue((position, radius, onTrigger));
        // 延迟处理队列
        instance.CallDeferred("_ProcessPendingAOEs");
    }

    public static void CreateExplosionEffect(Vector2 position, float level)
    {
        PackedScene explosionEffectScene = ResourceManager.GetScene(SceneResourceType.ExplosionEffect);
        OneTimeEffect explosionEffect = explosionEffectScene.Instantiate<OneTimeEffect>();
        instance.AddChild(explosionEffect);
        explosionEffect.GlobalPosition = position;
    }

    // 新增处理队列的方法
    private void _ProcessPendingAOEs()
    {
        while (pendingAOEs.Count > 0)
        {
            var (position, radius, onTrigger) = pendingAOEs.Dequeue();
            PackedScene aoeTriggerScene = ResourceManager.GetScene(SceneResourceType.AOE_Trigger);
            AOE_Trigger_Entity aoeTrigger = aoeTriggerScene.Instantiate<AOE_Trigger_Entity>();
            aoeTrigger.GlobalPosition = position;
            aoeTrigger.SetRadius(radius);
            aoeTrigger.OnTrigger = onTrigger;
            AddChild(aoeTrigger);
        }
    }

    public static void CreateExplosion(Vector2 position, float level)
    {
        CreateAOE_Trigger(position, (int)(level * 5), (Entity entity) => {
            entity.OnHit(level * 20);
        });
    }
}

