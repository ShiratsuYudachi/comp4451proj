using Godot;
using System;

public partial class SpellPieceIcon : TextureRect
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}


	public void setIconForSpellPiece(string name){
		this.Texture = ResourceManager.GetTexture(TextureResourceType.SpellPieceIcon, name);
		if (this.Texture != null) {
			// Check if texture size is not 32x32
			if (this.Texture.GetWidth() != 32 || this.Texture.GetHeight() != 32) {
				// Scale to fit 32x32
				this.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
			}
		}
		// this.Modulate = new Color(1, 1, 1, 0.8f);
	}

	public void clear(){
		this.Texture = null;
	}
}
