using Godot;
using System;

public partial class DPad : Control
{
	TextureButton UpButton;
	TextureButton DownButton;
	TextureButton LeftButton;
	TextureButton RightButton;
	TextureButton NoneButton;

	Texture2D DirectionNormal;
	Texture2D DirectionPressed;
	Texture2D NoneNormal;
	Texture2D NonePressed;

	Direction CurrentDirection = Direction.NONE;

	public enum Direction {
		NONE,
		UP,
		DOWN,
		LEFT,
		RIGHT
	}

	public override void _Ready()
	{
		UpButton = GetNode<TextureButton>("UpButton");
		DownButton = GetNode<TextureButton>("DownButton");
		LeftButton = GetNode<TextureButton>("LeftButton");
		RightButton = GetNode<TextureButton>("RightButton");
		NoneButton = GetNode<TextureButton>("NoneButton");

		UpButton.Connect("button_down", new Callable(this, nameof(SetDirectionUp)));
		DownButton.Connect("button_down", new Callable(this, nameof(SetDirectionDown)));
		LeftButton.Connect("button_down", new Callable(this, nameof(SetDirectionLeft)));
		RightButton.Connect("button_down", new Callable(this, nameof(SetDirectionRight)));
		NoneButton.Connect("button_down", new Callable(this, nameof(SetDirectionNone)));

		DirectionNormal = UpButton.TextureNormal;
		DirectionPressed = UpButton.TexturePressed;
		NoneNormal = NoneButton.TextureNormal;
		NonePressed = NoneButton.TexturePressed;

		SetDirection(Direction.NONE);
	}

	
	public override void _Process(double delta)
	{
	}

	public void SetDirection(Direction direction) {
		CurrentDirection = direction;
		switch (direction) {
			case Direction.UP:
				UpButton.TextureNormal = DirectionPressed;
				DownButton.TextureNormal = DirectionNormal;
				LeftButton.TextureNormal = DirectionNormal;
				RightButton.TextureNormal = DirectionNormal;
				NoneButton.TextureNormal = NoneNormal;
				break;
			case Direction.DOWN:
				UpButton.TextureNormal = DirectionNormal;
				DownButton.TextureNormal = DirectionPressed;
				LeftButton.TextureNormal = DirectionNormal;
				RightButton.TextureNormal = DirectionNormal;
				NoneButton.TextureNormal = NoneNormal;
				break;
			case Direction.LEFT:
				UpButton.TextureNormal = DirectionNormal;
				DownButton.TextureNormal = DirectionNormal;
				LeftButton.TextureNormal = DirectionPressed;
				RightButton.TextureNormal = DirectionNormal;
				NoneButton.TextureNormal = NoneNormal;
				break;
			case Direction.RIGHT:
				UpButton.TextureNormal = DirectionNormal;
				DownButton.TextureNormal = DirectionNormal;
				LeftButton.TextureNormal = DirectionNormal;
				RightButton.TextureNormal = DirectionPressed;
				NoneButton.TextureNormal = NoneNormal;
				break;
			case Direction.NONE:
				UpButton.TextureNormal = DirectionNormal;
				DownButton.TextureNormal = DirectionNormal;
				LeftButton.TextureNormal = DirectionNormal;
				RightButton.TextureNormal = DirectionNormal;
				NoneButton.TextureNormal = NonePressed;
				break;
		}
	}

	public void SetDirectionUp() {
		SetDirection(Direction.UP);
	}

	public void SetDirectionDown() {
		SetDirection(Direction.DOWN);
	}

	public void SetDirectionLeft() {
		SetDirection(Direction.LEFT);
	}

	public void SetDirectionRight() {
		SetDirection(Direction.RIGHT);
	}

	public void SetDirectionNone() {
		SetDirection(Direction.NONE);
	}
	

	public Direction GetDirection() {
		return CurrentDirection;
	}

	public void refresh(){
		SetDirection(CurrentDirection);
	}
}
