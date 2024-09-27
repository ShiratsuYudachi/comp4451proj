using Godot;
using System;
using System.Runtime.CompilerServices;





public partial class SpellCaster : Node2D
{
    public SpellEvaluationTreeNode testEvaluationTree;

	float ManaMax = 5000;
	float Mana = 5000;
	float ManaRegenSpeed = 100; // Mana per second


	public override void _Ready()
	{
		GD.Print("SpellExecutor ready");
        testEvaluationTree = new SpellEvaluationTreeNode(new Blink());
        testEvaluationTree.childrenSpellPieces[0] = new SpellEvaluationTreeNode(new SelectCaster());
        testEvaluationTree.childrenSpellPieces[1] = new SpellEvaluationTreeNode(new VectorMinus());
        testEvaluationTree.childrenSpellPieces[1].childrenSpellPieces[0] = new SpellEvaluationTreeNode(new SelectMousePos());
        testEvaluationTree.childrenSpellPieces[1].childrenSpellPieces[1] = new SpellEvaluationTreeNode(new GetEntityPos());
        testEvaluationTree.childrenSpellPieces[1].childrenSpellPieces[1].childrenSpellPieces[0] = new SpellEvaluationTreeNode(new SelectCaster());
	}

	public void Cast()
	{
		GD.Print("Executing spell");
        testEvaluationTree.Evaluate(this);
	}

	public bool TryToConsumeMana(int amount)
	{
		if (Mana >= amount)
		{
			Mana -= amount;
			GD.Print("Consumed " + amount + " mana. " + Mana + " left.");
			return true;
		}
		GD.Print("Not enough mana! " + Mana + " < " + amount);
		return false;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Attack"))
		{
			Cast();
		}
		Mana += ManaRegenSpeed * (float)delta;
		
		if (Mana > ManaMax)
		{
			Mana = ManaMax;
		}
		//GD.Print("Mana: " + Mana);
	}
}