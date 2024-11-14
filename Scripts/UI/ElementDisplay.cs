using Godot;
using System;
using Chemistry;
using System.Collections.Generic;

public partial class ElementDisplay : Node2D
{
	HBoxContainer hBoxContainer;
	Entity parentEntity;

	[Export]
	public PackedScene textureRectScene; // TODO


	public float updateInterval = 1f;


	Timer updateTimer;
	HashSet<Element> lastElements = new HashSet<Element>();

	public override void _Ready()
	{
		hBoxContainer = GetNode<HBoxContainer>("HBoxContainer");
		parentEntity = GetParent<Entity>();
		
		// Setup timer
		updateTimer = new Timer();
		AddChild(updateTimer);
		updateTimer.WaitTime = 0.1f; // Update every 0.1 seconds
		updateTimer.Timeout += _updateElementDisplay;
		updateTimer.Start();
	}

	private void _updateElementDisplay()
	{
		
		var currentElements = new HashSet<Element>(parentEntity.reactor.GetActiveElements());
		
		// Remove disappeared elements
		foreach (Element element in lastElements)
		{
			if (!currentElements.Contains(element))
			{
				var elementNode = hBoxContainer.GetNodeOrNull(element.ToString());
				if (elementNode != null)
				{
					elementNode.QueueFree();
				}
			}
		}
		
		// Add new elements
		foreach (Element element in currentElements)
		{
			if (!lastElements.Contains(element))
			{
				// Instance the scene instead of creating new TextureRect
				var textureRect = textureRectScene.Instantiate<TextureRect>();
				textureRect.Name = element.ToString();
				textureRect.Texture = ResourceManager.GetTexture(ResourceType.ElementIcon, element.ToString());
				hBoxContainer.AddChild(textureRect);
			}
		}
		
		lastElements = currentElements;
	}
}
