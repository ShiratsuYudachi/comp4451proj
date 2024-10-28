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
            bindedSpell.Evaluate(caster);
        }
    }
}

class OnBulletIsNear : Trigger{
    public OnBulletIsNear(SpellEvaluationTreeNode bindedSpell, SpellCaster caster) : base(bindedSpell, caster){

    }
    public override bool Check(){
        MonsterDetector monsterDetector = GameScene.player.GetNode<MonsterDetector>("MonsterDetector");
        
        foreach (Bullet bullet in monsterDetector.bulletList){
            if (bullet.caster != GameScene.player && bullet.GlobalPosition.DistanceTo(caster.GlobalPosition) < 25){
                return true;
            }
        }
        return false;
    }
}   