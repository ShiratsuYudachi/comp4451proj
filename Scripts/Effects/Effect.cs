using Godot;
using System;


public abstract class Effect{
    protected Entity target;

    private PackedScene _effectScene;
    private Node2D _effect;

    abstract protected string effectScenePath{get;}

    public double duration;

    public Effect(Entity target, double duration){
        this.target = target;
        this.duration = duration;
        _effectScene = GD.Load<PackedScene>(effectScenePath);
        if (_effectScene == null){
            GD.PushError("Effect scene not found: " + effectScenePath);
        }
        _effect = _effectScene.Instantiate<Node2D>();
        target.AddChild(_effect);
    }

    public virtual void Update(double delta){
        duration -= delta;
    }

    public virtual void OnRemove(){
        _effect.QueueFree();
    }
}


public class BurningEffect : Effect{
    protected override string effectScenePath => "res://Scenes/Effects/BurningEffect.tscn";

    private float timer = 0;
    private float timerMax = 0.99f;

    public BurningEffect(Entity target, double duration) : base(target, duration){
        timer = timerMax;
    }

    public override void Update(double delta){
        timer -= (float)delta;
        if (timer <= 0){
            target.reactor.AddMaterial(Chemistry.Element.Pyro, 1);
            target.OnHit(50);
            timer = timerMax;
        }
        base.Update(delta);
    }
}
