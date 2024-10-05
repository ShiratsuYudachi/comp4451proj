using Godot;
using System;
using System.Runtime.CompilerServices;





public partial class SpellCaster : Node2D
{
    public SpellEvaluationTreeNode testEvaluationTree;

	public float ManaMax = 5000;
	public float Mana = 5000;
	public float ManaRegenSpeed = 100; // Mana per second


	public override void _Ready()
	{
		GD.Print("SpellExecutor ready");
        testEvaluationTree = new SpellEvaluationTreeNode(new Blink());
        testEvaluationTree.childrenSpellPieces[0] = new SpellEvaluationTreeNode(new SelectCaster());
        testEvaluationTree.childrenSpellPieces[1] = new SpellEvaluationTreeNode(new VectorMinus());
        testEvaluationTree.childrenSpellPieces[1].childrenSpellPieces[0] = new SpellEvaluationTreeNode(new SelectMousePos());
        testEvaluationTree.childrenSpellPieces[1].childrenSpellPieces[1] = new SpellEvaluationTreeNode(new GetEntityPos());
        testEvaluationTree.childrenSpellPieces[1].childrenSpellPieces[1].childrenSpellPieces[0] = new SpellEvaluationTreeNode(new SelectCaster());
		testEvaluationTree.PrintTree();
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
			return true;
		}
		return false;
	}

	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Cast"))
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