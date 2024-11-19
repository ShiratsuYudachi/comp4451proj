using Godot;
using System;

public partial class SpellEditor : GridContainer
{
	[Export]
	public SpellPicker spellPicker;

	[Export]
	public PackedScene spellPieceConfigPanelScene;

	[Export]
	public LineEdit spellNameInput;

	[Export]
	public Button compileButton;

	[Export]
	public Button clearButton;

	[Export]
	public PackedScene spellEditorBoxScene;


	int EditorSize = 4; // nxn

	

	public SpellPieceConfigPanel spellPieceConfigPanel;


	public enum LastMouseButton{
		Left,
		Right,
		Middle
	}
	public LastMouseButton lastMouseButton = LastMouseButton.Left;


	private int currentActivePanelIndex = -1;

	
	public override void _Ready()
	{
		compileButton.Connect("pressed", new Callable(this, nameof(CompileSpell)));
		clearButton.Connect("pressed", new Callable(this, nameof(ClearEditor)));
		for (int i = 0; i < EditorSize*EditorSize; i++){
			SpellEditorBox spellEditorBox = spellEditorBoxScene.Instantiate<SpellEditorBox>();
			this.AddChild(spellEditorBox);
		}
	}

	
	public override void _Process(double delta)
	{
		if (Input.GetMouseButtonMask() == MouseButtonMask.Left){
			lastMouseButton = LastMouseButton.Left;
		}
		else if (Input.GetMouseButtonMask() == MouseButtonMask.Right){
			lastMouseButton = LastMouseButton.Right;
		}
		else if (Input.GetMouseButtonMask() == MouseButtonMask.Middle){
			lastMouseButton = LastMouseButton.Middle;
		}

		else if (Input.IsActionJustPressed("Cancel"))
		{
			closeExistingPanel();
		}

		if (Input.IsActionJustPressed("EnterN"))
		{
			CompileSpell();
		}
	}

	public SpellPicker getSpellPicker(){
		return spellPicker;
	}

	public void ClearEditor(){
		foreach (SpellEditorBox spellEditorBox in this.GetChildren()){
			spellEditorBox.clear();
		}
		spellNameInput.Text = "";
	}

	public void setSpellPieceConfigPanelAt(SpellEditorBox spellEditorBox){
		if (spellEditorBox.spellPiece == null) return;
		closeExistingPanel();
		spellPieceConfigPanel = spellPieceConfigPanelScene.Instantiate<SpellPieceConfigPanel>();
		this.GetParent().AddChild(spellPieceConfigPanel);
		spellPieceConfigPanel.Position = spellEditorBox.Position*this.Scale + this.Position;
		spellPieceConfigPanel.setupParamSelectorsAt(spellEditorBox);
	}

	public void setSpellPickerAt(SpellEditorBox spellEditorBox){
		closeExistingPanel();
		spellPicker.Position = spellEditorBox.Position*this.Scale + this.Position;
	}

	public void closeExistingPanel(){
		if (spellPieceConfigPanel != null && IsInstanceValid(spellPieceConfigPanel)){
			spellPieceConfigPanel.QueueFree();
		}
		spellPicker.resetPosition();
	}

	

	private Vector2 getBoxCoordinateInGrid(SpellEditorBox spellEditorBox){
		return new Vector2(spellEditorBox.GetIndex()%this.Columns, spellEditorBox.GetIndex()/this.Columns);
	}

	private SpellEditorBox getBoxAtCoordinate(Vector2 coordinate){
		foreach (SpellEditorBox spellEditorBox in this.GetChildren()){
			if (getBoxCoordinateInGrid(spellEditorBox) == coordinate) return spellEditorBox;
		}
		return null;
	}

	public void CompileSpell(){
		SpellEvaluationTreeNode result = null;
		SpellEditorBox rootBox = null;
		
		foreach (SpellEditorBox spellEditorBox in this.GetChildren()){
			if (spellEditorBox.spellPiece is ExecutorSpellPiece){
				rootBox = spellEditorBox;
				break;
			}
		}
		if (rootBox == null){
			SpellWorkspace.showMessage("No valid executor spell piece found");
		}
		

		result = CompileBox(rootBox);
		if (spellNameInput.Text == ""){
			SpellWorkspace.showMessage("No spell name given");
		}
		else if (result == null){
			SpellWorkspace.showMessage("Error in spell compilation");
		}
		else{
			result.PrintTree();
			SpellWorkspace.showMessage("Spell compiled!");
			GameScene.playerSpellStorage.AddSpell(spellNameInput.Text, result);
			SaveWorkspace(spellNameInput.Text);
			SpellWorkspace.Refresh();
		}
	}

	public SpellEvaluationTreeNode CompileBox(SpellEditorBox spellEditorBox){
		if (spellEditorBox.spellPiece is SelectorSpellPiece) return new SpellEvaluationTreeNode(spellEditorBox.spellPiece);
		SpellEvaluationTreeNode root = new SpellEvaluationTreeNode(spellEditorBox.spellPiece);
		root.childrenSpellPieces = new SpellEvaluationTreeNode[root.rootSpellPiece.ParamList.Length];

		for (int i = 0; i < root.childrenSpellPieces.Length; i++){
			DPad.Direction paramDirection = spellEditorBox.SpellPieceParamDirection[i];
			Vector2 targetBoxCoordinateAtGrid = getBoxCoordinateInGrid(spellEditorBox);
			switch(paramDirection){
				case DPad.Direction.UP:
					targetBoxCoordinateAtGrid.Y -= 1;
					break;
				case DPad.Direction.DOWN:
					targetBoxCoordinateAtGrid.Y += 1;
					break;
				case DPad.Direction.LEFT:
					targetBoxCoordinateAtGrid.X -= 1;
					break;
				case DPad.Direction.RIGHT:
					targetBoxCoordinateAtGrid.X += 1;
					break;
				case DPad.Direction.NONE:
					SpellWorkspace.showMessage("No parameter direction selected for " + spellEditorBox.selectedSpellPieceName + " at " + getBoxCoordinateInGrid(spellEditorBox));
					return null;
			}
			SpellEditorBox targetBox = getBoxAtCoordinate(targetBoxCoordinateAtGrid);
			root.childrenSpellPieces[i] = CompileBox(targetBox);
		}

		return root;
	}

	


	// public TextureRect NewSpellPieceIcon(string name){
	// 	TextureRect icon = spellPieceIcon.Instantiate<TextureRect>();
	// 	icon.Texture = GD.Load<Texture2D>("res://images/spells/"+name+".png");
	// 	return icon;
	// }

	public Godot.Collections.Dictionary ToJSON()
	{
		var jsonObj = new Godot.Collections.Dictionary();
		var gridArray = new Godot.Collections.Array();
		
		// Save spell name
		jsonObj["spellName"] = spellNameInput.Text;
		
		// Save each box in the grid
		foreach (SpellEditorBox box in this.GetChildren())
		{
			var boxData = new Godot.Collections.Dictionary();
			
			// Save SpellPiece if exists
			if (box.spellPiece != null)
			{
				boxData["spellPiece"] = box.spellPiece.ToJSON();
				
				// Save parameter directions
				var directionsArray = new Godot.Collections.Array();
				foreach (var direction in box.SpellPieceParamDirection)
				{
					directionsArray.Add((int)direction);
				}
				boxData["paramDirections"] = directionsArray;
			}
			else
			{
				boxData["spellPiece"] = new Godot.Collections.Dictionary();
			}
			
			gridArray.Add(boxData);
		}
		
		jsonObj["grid"] = gridArray;
		return jsonObj;
	}

	public void LoadFromJSON(Godot.Collections.Dictionary jsonObj)
	{
		// Clear current state
		ClearEditor();
		
		// Load spell name
		spellNameInput.Text = (string)jsonObj["spellName"];
		
		// Load grid data
		var gridArray = (Godot.Collections.Array)jsonObj["grid"];
		var boxes = this.GetChildren();
		
		for (int i = 0; i < gridArray.Count; i++)
		{
			var boxData = (Godot.Collections.Dictionary)gridArray[i];
			var box = (SpellEditorBox)boxes[i];
			
			if (boxData.ContainsKey("paramDirections"))
			{
				// Load SpellPiece
				var spellPieceData = (Godot.Collections.Dictionary)boxData["spellPiece"];
				box.spellPiece = SpellPiece.FromJSON(spellPieceData);
				box.selectedSpellPieceName = box.spellPiece.GetType().Name;
				box.spellPieceIcon.setIconForSpellPiece(box.selectedSpellPieceName);
				
				// Load parameter directions
				var directionsArray = (Godot.Collections.Array)boxData["paramDirections"];
				for (int j = 0; j < directionsArray.Count; j++)
				{
					box.SpellPieceParamDirection[j] = (DPad.Direction)(int)directionsArray[j];
				}
				box.paramSourceDisplay.updateParamSourceDisplay(box.SpellPieceParamDirection);
			}
		}
	}

	public void SaveWorkspace(string spellName)
	{
		string dirPath = System.IO.Path.Combine(
			System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),
			"SpellCompiler"
		);
		System.IO.Directory.CreateDirectory(dirPath);

		var jsonDict = new Godot.Collections.Dictionary();
		jsonDict["workspace"] = ToJSON();
		
		
		if (GameScene.playerSpellStorage.spells.ContainsKey(spellName))
		{
			jsonDict["spell"] = GameScene.playerSpellStorage.spells[spellName].ToJSON();
		}

		string jsonPath = System.IO.Path.Combine(dirPath, $"{spellName}.json");
		string jsonString = Json.Stringify(jsonDict);
		System.IO.File.WriteAllText(jsonPath, jsonString);
	}

	public void LoadWorkspace(string spellName)
	{
		string dirPath = System.IO.Path.Combine(
			System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments),
			"SpellCompiler"
		);
		string jsonPath = System.IO.Path.Combine(dirPath, $"{spellName}.json");

		if (!System.IO.File.Exists(jsonPath)) return;

		string jsonString = System.IO.File.ReadAllText(jsonPath);
		var jsonDict = Json.ParseString(jsonString).AsGodotDictionary();

		// Load workspace state
		LoadFromJSON((Godot.Collections.Dictionary)jsonDict["workspace"]);
		
		// Load spell tree
		if (jsonDict.ContainsKey("spell"))
		{
			var spellTree = SpellEvaluationTreeNode.loadJSON((Godot.Collections.Dictionary)jsonDict["spell"]);
			GameScene.playerSpellStorage.spells[spellName] = spellTree;
		}
	}
}
