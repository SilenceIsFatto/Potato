using Godot;
using System;

public partial class SkyController : TextureRect
{
	private TextureRect sky;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		sky = GetNode<TextureRect>("sky");
	} 

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		Vector2 oldPosition = sky.Position;
		sky.Position = new Vector2(1000, 1000) + oldPosition;
	}
}
