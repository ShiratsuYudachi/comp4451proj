using Godot;
abstract class Trigger{
    public SpellEvaluationTreeNode bindedSpell;
    public SpellCaster caster;

    public Trigger(SpellEvaluationTreeNode bindedSpell, SpellCaster caster){
        this.bindedSpell = bindedSpell;
        this.caster = caster;
    }
    public abstract bool Check();
    public void update(){
        if(Check()){
            // GD.Print("Trigger " + this.GetType().Name + " triggered");
            bindedSpell.Evaluate(caster);
        }
    }
}

class OnBulletIsNear : Trigger{
    public OnBulletIsNear(SpellEvaluationTreeNode bindedSpell, SpellCaster caster) : base(bindedSpell, caster){

    }

    private CircularBuffer<IMassEntity> lastCheckResults = new CircularBuffer<IMassEntity>(20);

    bool avoidDuplicate = true;
    public override bool Check(){
        EntityDetector monsterDetector = GameScene.player.GetNode<EntityDetector>("EntityDetector");
        if (monsterDetector == null){
            GD.PrintErr("Trigger OnBulletIsNear of entity " + caster.GetParent().GetName() + " failed: No EntityDetector found");
            return false;
        }
        
        foreach (IMassEntity massEntity in monsterDetector.entityList){
            if (massEntity is Bullet){
                if (((Bullet)massEntity).caster != GameScene.player && massEntity.massPosition.DistanceTo(caster.GlobalPosition) < 25){
                    if (avoidDuplicate){
                        if (lastCheckResults.Contains(massEntity)){
                        continue;
                        }

                        lastCheckResults.Add(massEntity);
                    }
                    return true;
                }
            }
        }
        
        return false;
    }
}   