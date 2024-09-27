using Godot;
using System;
using System.Runtime.CompilerServices;

public class SpellEvaluationTreeNode
{
    public SpellPiece rootSpellPiece;

    public SpellEvaluationTreeNode[] childrenSpellPieces;

    public SpellVariable Evaluate(SpellExecutor spellExecutor){
        SpellVariable[] castingParams = new SpellVariable[rootSpellPiece.ParamList.Length];
        
        if (rootSpellPiece is SelectorSpellPiece){
            return ((SelectorSpellPiece)rootSpellPiece).Select(spellExecutor);
        }
        
        for (int i = 0; i < castingParams.Length; i++)
        {
            castingParams[i] = childrenSpellPieces[i].Evaluate(spellExecutor);
        }
        
        if (rootSpellPiece is OperatorSpellPiece){
            return ((OperatorSpellPiece)rootSpellPiece).Operate(castingParams);
        }

        if (rootSpellPiece is ExecutorSpellPiece){
            ((ExecutorSpellPiece)rootSpellPiece).Execute(castingParams);
        }
        return new SpellVariable(SpellVariableType.NONE, null);
    }

    public SpellEvaluationTreeNode(SpellPiece root){
        this.rootSpellPiece = root;
        this.childrenSpellPieces = new SpellEvaluationTreeNode[root.ParamList.Length];
    }
}


public partial class SpellExecutor : Node2D
{
    public SpellEvaluationTreeNode testEvaluationTree;
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

	public void Execute()
	{
		GD.Print("Executing spell");
        testEvaluationTree.Evaluate(this);
	}

    
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Attack"))
		{
			Execute();
		}
	}
}