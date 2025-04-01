using Godot;
using System;
using System.Threading.Tasks;

public partial class HandlePotato : Sprite2D
{
	private bool debug = true;
	
	private TextureRect dirt;
	private TextureRect dirtTransparent;
	
	private float growthSpeedConst = 0.0001f;
	private float growthSpeed = 0.0001f;
	
	private Sprite2D potato;
	private Vector2 potatoOffsetDefault;
	private Vector2 potatoScaleDefault;
	private float potatoRotationDefault;
	
	private Camera2D camera;
	private int cameraShakeStrength = 5;
	private int cameraShakeFade = 5;
	Random cameraShakeRandom = new Random();
	private int cameraShake = 0;
	
	private PotatoUI potatoUI;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		dirt = GetNode<TextureRect>("dirt");
		dirtTransparent = GetNode<TextureRect>("dirt_transparent");
		potato = GetNode<Sprite2D>("potato");
		potatoUI = GetNode<PotatoUI>("PotatoUi");
		camera = GetNode<Camera2D>("camera");
		
		HandleOverlayDirt();
		
		potatoOffsetDefault = potato.Offset;
		potatoScaleDefault = potato.Scale;
		potatoRotationDefault = potato.Rotation;
		
		if (debug) {growthSpeedConst = 0.001f;};
		
		GD.Print($"Debug is: {debug}");
		GD.Print($"Growth Speed: {growthSpeed}");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		HandleGrowth();
	}
	
	private Vector2 RandomOffset()
	{
		return new Vector2(cameraShakeRandom.Next(-cameraShakeStrength, cameraShakeStrength), cameraShakeRandom.Next(-cameraShakeStrength, cameraShakeStrength));
	}
	
	private float RandomRotation(float baseRotation)
	{
		return baseRotation + (cameraShakeStrength / 7);
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton eventMouse && eventMouse.Pressed)
		{
			HandleClick(eventMouse);
		}
	}
	
	private void HandleShake()
	{
		potato.Offset = RandomOffset();
		potato.Rotation = RandomRotation(potato.Rotation);
	}
	
	private async void ShakeSprite()
	{
		for (int i = 0; i <= 10; i++) 
		{
			HandleShake();
			await Task.Delay(10);
		}
	}
	
	private void HandleClick(InputEventMouseButton eventMouse)
	{
		Vector2 mousePosition = eventMouse.Position;
		GD.Print("Mouse Click at: ", mousePosition);
		
		growthSpeed = growthSpeed + growthSpeedConst / 10;
		
		GD.Print(growthSpeed);
		
		ShakeSprite();
	}
	
	private void HandleOverlayDirt()
	{
		var dirtSize = dirt.Size;
		var dirtPosition =  dirt.Position;
		var dirtScale = dirt.Scale;
		GD.Print($"Gathered dirt node data. Size: {dirtSize} | Position: {dirtPosition} | Scale: {dirtScale}");
		
		// Sync dirtTransparent
		dirtTransparent.Size = dirtSize;
		dirtTransparent.Position = dirtPosition;
		dirtTransparent.Scale = dirtScale;
		GD.Print($"Set dirtTransparent node data. Size: {dirtSize} | Position: {dirtPosition} | Scale: {dirtScale}");
	}
	
	private float GetOpacity(TextureRect texture)
	{
		float textureOpacity = texture.Modulate[3];
		
		return textureOpacity;
	}
	
	private void SetOpacity(TextureRect texture, float alpha)
	{
		Godot.Color textureModulate = texture.Modulate;
		Godot.Color textureOpacity = new Godot.Color(textureModulate[0], textureModulate[1], textureModulate[2], alpha);
		texture.Modulate = textureOpacity;
	}
	
	private void SetOpacitySmooth(TextureRect texture, float alpha)
	{
		Godot.Color textureOpacityOriginal = texture.Modulate;
		float textureOpacityNew = textureOpacityOriginal[3] - alpha;
		
		Godot.Color textureOpacity = new Godot.Color(
			textureOpacityOriginal[0],
			textureOpacityOriginal[1],
			textureOpacityOriginal[2],
			textureOpacityNew
		);
		
		texture.Modulate = textureOpacity;
	}
	
	private void SetScale(Sprite2D texture, Vector2 scale)
	{
		texture.Scale = scale;
	}
	
	private void SetScaleSmooth(Sprite2D texture, Vector2 scale)
	{
		Vector2 scaleOriginal = texture.Scale;
		Vector2 scaleNew = scaleOriginal + scale;
		
		texture.Scale = scaleNew;
	}
	
	private async void HandleGrowth()
	{
		SetOpacitySmooth(dirtTransparent, growthSpeed);
		SetScaleSmooth(potato, new Vector2(growthSpeed, growthSpeed));
		if (GetOpacity(dirtTransparent) <= 0)
		{
			ResetPotato();
		}
	}
	
	private void ResetPotato()
	{
		GD.Print("Potato has finished growing!");
		potatoUI.potatoes ++;
		
		potatoUI.SetPotatoCount(potatoUI.GetPotatoCount());
		SetOpacity(dirtTransparent, 1);
		SetScale(potato, potatoScaleDefault);
		
		growthSpeed = growthSpeedConst;
		potato.Offset = potatoOffsetDefault;
		//potato.Rotation = potatoRotationDefault;
		potato.Rotation = new Random().Next(1, 360);
	}
}
