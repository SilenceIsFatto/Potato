using Godot;
using System;

public partial class PotatoUI : Control
{
	public int potatoes;
	private Label _potatoesGrown;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		_potatoesGrown = GetNode<Label>("potatoes");
		potatoes = 0;
	}
	
	public string FormatPotatoCount(int count)
	{
		string potatoHeader = "Potatoes Harvested";
		
		string potatoCount = $"{potatoHeader}\n\n{count}";
		
		return potatoCount;
	}
	
	public void SetPotatoCount(int count)
	{
		string potatoCount = FormatPotatoCount(count);
		
		_potatoesGrown.Text = potatoCount;
	}
	
	public int GetPotatoCount()
	{
		return potatoes;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
}
