using Godot;
using System;
using System.Runtime.CompilerServices;

public class SpellEvaluateTreeNode
{
    public SpellPiece evaluateRoot;

    public SpellEvaluateTreeNode[] evaluateChildren;

    public SpellVariable Evaluate(SpellExecutor spellExecutor){
        SpellVariable[] castingParams = new SpellVariable[evaluateRoot.ParamList.Length];
        
        if (evaluateRoot is SelectorSpellPiece){
            return ((SelectorSpellPiece)evaluateRoot).Select(spellExecutor);
        }
        
        for (int i = 0; i < castingParams.Length; i++)
        {
            castingParams[i] = evaluateChildren[i].Evaluate(spellExecutor);
        }
        
        if (evaluateRoot is OperatorSpellPiece){
            return ((OperatorSpellPiece)evaluateRoot).Operate(castingParams);
        }

        if (evaluateRoot is ExecutorSpellPiece){
            ((ExecutorSpellPiece)evaluateRoot).Execute(castingParams);
        }
        return new SpellVariable(SpellVariableType.NONE, null);
    }
}


public partial class SpellExecutor : Node
{
    public SpellEvaluateTreeNode testEvaluateTree;
	public override void _Ready()
	{
		GD.Print("SpellExecutor ready");
        testEvaluateTree = new SpellEvaluateTreeNode();
        testEvaluateTree.evaluateRoot = new Blink();
        testEvaluateTree.evaluateChildren = new SpellEvaluateTreeNode[2];
        testEvaluateTree.evaluateChildren[0] = new SpellEvaluateTreeNode();
        testEvaluateTree.evaluateChildren[0].evaluateRoot = new SelectCaster();
        testEvaluateTree.evaluateChildren[1] = new SpellEvaluateTreeNode();
        testEvaluateTree.evaluateChildren[1].evaluateRoot = new Vector2ConstantSpellPiece(new Vector2(50, 50));
	}

	public void Execute()
	{
		GD.Print("Executing spell");
        testEvaluateTree.Evaluate(this);
	}

    
	public override void _Process(double delta)
	{
		if (Input.IsActionJustPressed("Attack"))
		{
			Execute();
		}
	}
}