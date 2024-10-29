using Godot;
using System;
using System.Runtime.CompilerServices;
using System.Collections.Generic;

public partial class SpellCaster : Node2D
{
	public SpellEvaluationTreeNode testEvaluationTree1, testEvaluationTree2, testEvaluationTree3, blinkUp;

	public float ManaMax = 20000;
	public float Mana = 20000;
	public float ManaRegenSpeed = 2000; // Mana per second

	List<Trigger> spellTriggers = new List<Trigger>();


	public override void _Ready()
	{
		GD.Print("SpellExecutor ready");
		//Blink
		testEvaluationTree1 = new SpellEvaluationTreeNode(new Blink());
		testEvaluationTree1.childrenSpellPieces[0] = new SpellEvaluationTreeNode(new SelectCaster());
		testEvaluationTree1.childrenSpellPieces[1] = new SpellEvaluationTreeNode(new VectorMinus());
		testEvaluationTree1.childrenSpellPieces[1].childrenSpellPieces[0] = new SpellEvaluationTreeNode(new SelectMousePos());
		testEvaluationTree1.childrenSpellPieces[1].childrenSpellPieces[1] = new SpellEvaluationTreeNode(new GetEntityPos());
		testEvaluationTree1.childrenSpellPieces[1].childrenSpellPieces[1].childrenSpellPieces[0] = new SpellEvaluationTreeNode(new SelectCaster());
		//GD.Print(testEvaluationTree1.ToJSON());

		
		
		
		//MassAddMotion
		testEvaluationTree2 = new SpellEvaluationTreeNode(new MassAddMotion());
		testEvaluationTree2.childrenSpellPieces[0] = new SpellEvaluationTreeNode(new SelectCaster());
		testEvaluationTree2.childrenSpellPieces[1] = new SpellEvaluationTreeNode(new Vector2ConstantSpellPiece(new Vector2(3, 4)));
		testEvaluationTree2.PrintTree();
		GD.Print(testEvaluationTree2.ToJSON());
		//Heal
		testEvaluationTree3 = new SpellEvaluationTreeNode(new Heal());
		testEvaluationTree3.childrenSpellPieces[0] = new SpellEvaluationTreeNode(new SelectCaster());
		testEvaluationTree3.childrenSpellPieces[1] = new SpellEvaluationTreeNode(new IntConstantSpellPiece(114));

		//BlinkUp
		blinkUp = new SpellEvaluationTreeNode(new MassAddMotion());
		blinkUp.childrenSpellPieces[0] = new SpellEvaluationTreeNode(new SelectNearestBullet());
		blinkUp.childrenSpellPieces[1] = new SpellEvaluationTreeNode(new VectorMultiplication());
		blinkUp.childrenSpellPieces[1].childrenSpellPieces[0] = new SpellEvaluationTreeNode(new GetEntityVelocity());
		blinkUp.childrenSpellPieces[1].childrenSpellPieces[0].childrenSpellPieces[0] = new SpellEvaluationTreeNode(new SelectNearestBullet());
		blinkUp.childrenSpellPieces[1].childrenSpellPieces[1] = new SpellEvaluationTreeNode(new FloatConstantSpellPiece(-2f));
		spellTriggers.Add(new OnBulletIsNear(blinkUp, this));

		// 确保目录存在
		string dirPath = System.IO.Path.Combine(
			System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),
			"SpellCompiler"
		);
		System.IO.Directory.CreateDirectory(dirPath);
		
		// 保存测试树到JSON
		string jsonPath = System.IO.Path.Combine(dirPath, "testSpell.json");
		var jsonDict = testEvaluationTree2.ToJSON();
		string jsonString = Json.Stringify(jsonDict);
		System.IO.File.WriteAllText(jsonPath, jsonString);
		
		// 从JSON加载并打印树结构
		GD.Print("Original tree:");
		testEvaluationTree2.PrintTree();
		
		string loadedJsonString = System.IO.File.ReadAllText(jsonPath);
		var loadedDict = Json.ParseString(loadedJsonString).AsGodotDictionary();
		var loadedTree = SpellEvaluationTreeNode.loadJSON(loadedDict);
		
		GD.Print("\nLoaded tree:");
		loadedTree.PrintTree();
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

		foreach (Trigger trigger in spellTriggers){
			trigger.update();
		}
		//GD.Print("Mana: " + Mana);
	}

	

	public void Cast()
	{
		GD.Print("Executing spell");
		testEvaluationTree1.Evaluate(this);
		// testEvaluationTree2.Evaluate(this);
		// testEvaluationTree3.Evaluate(this);
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
}