using Godot;
using System;

public partial class PlayerMovement : CharacterBody2D
{
	[Export]
	private float MaxSpeed = 400f;
	[Export]
	private float DashSpeed = 800;
	[Export]
	private float DashDuration = .2f;
	[Export]
	private float DashCooldown = 2f;
	[Export]
	private float Acceleration = 5000f;
	[Export]
	private float DamageTime = 0.1f;
	[Export]
	private PackedScene ghostScene;
	[Export]
	private PackedScene dashSmoke;
	
	private bool isDashing = false;
	private Vector2 dashDirection = Vector2.Zero;
	
	private int Health = 100;
	
	private Timer dashCooldownTimer;
	private Timer dashDurationTimer;
	private Timer damageTimer;
	private Timer ghostTimer;	
	
	private AnimatedSprite2D _anim;
	private ShaderMaterial _flashMaterial;

	public void takeDamage(int amount)
	{
		_flashMaterial.SetShaderParameter("flash_red", true);
		damageTimer.Start(DamageTime);
		Health = Math.Max(0, Health - amount);
	}

	public override void _EnterTree()
	{
		SetMultiplayerAuthority(int.Parse(Name));
	}

	public override void _Ready()
	{
		// Animation
		_anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_anim.Animation = "idle";
		_flashMaterial = (ShaderMaterial)_anim.Material;
		
		// Dash Timers
		dashCooldownTimer = new Timer();
		dashDurationTimer = new Timer();
		ghostTimer = new Timer();
		damageTimer = new Timer();
		
		AddChild(dashCooldownTimer);
		AddChild(dashDurationTimer);
		AddChild(ghostTimer);
		AddChild(damageTimer);
		
		dashCooldownTimer.WaitTime = DashCooldown;
		dashDurationTimer.WaitTime = DashDuration;
		damageTimer.WaitTime = DamageTime;
		ghostTimer.WaitTime = DashDuration/4f;
		
		dashCooldownTimer.OneShot = true;
		dashDurationTimer.OneShot = true;
		ghostTimer.OneShot = false;
		damageTimer.OneShot = true;
		
		ghostTimer.Timeout += OnGhostTimerTimeout;
		damageTimer.Timeout += OnTakeDamage;
		dashCooldownTimer.Timeout += OnDashCooldownTimeout;
		dashDurationTimer.Timeout += OnDashDurationTimeout;
	}
	
	
	
	public override void _Process(double delta)
	{
		if (IsMultiplayerAuthority())
		{
			Vector2 inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
			
			// Spell Logic
			if (Input.IsActionJustPressed("cast"))
			{
				GD.Print("You cast a spell!");
			}
			
			// Movement Code
			if (isDashing)
				{
					if (dashDurationTimer.IsStopped())
					{
						isDashing = false;
						ghostTimer.Stop();
						_flashMaterial.SetShaderParameter("flash_white", false);
					} else {
						Velocity = DashSpeed * dashDirection;
						_flashMaterial.SetShaderParameter("flash_white", true);
					}
				}
			else if (inputDirection != Vector2.Zero)
			{
				inputDirection = inputDirection.Normalized();
				
				if ((Input.IsActionJustPressed("dash") && dashCooldownTimer.IsStopped()))
				{
					dashCooldownTimer.Start(DashCooldown);
					dashDurationTimer.Start(DashDuration);
					dashDirection = inputDirection;
					Velocity = DashSpeed * dashDirection;
					isDashing = true;
					ghostTimer.Start();
				} 
				 else {
					_flashMaterial.SetShaderParameter("flash_white", false);
					Velocity = Velocity.MoveToward(inputDirection * MaxSpeed, Acceleration * (float)delta);
				}
				
				if (inputDirection.Y != 0) 
				{
					_anim.Animation = inputDirection.Y > 0 ? "run" : "runUp";
				}
				else if (inputDirection.X != 0)
				{
					_anim.Animation = "runSide";
					_anim.FlipH = inputDirection.X < 0;
				}

				if (!_anim.IsPlaying())
					_anim.Play();
			}
			else
			{
				//GD.Print(dashDurationTimer.TimeLeft);
				Velocity = Vector2.Zero;
				_anim.Stop();
				_anim.Animation = "idle";
			}

		}

		MoveAndSlide();
	}
		
	private void OnDashDurationTimeout() {
		SpawnGhost();
		SpawnSmoke();
	}
	private void OnDashCooldownTimeout() {
		
	}
	private void OnGhostTimerTimeout()
	{
		SpawnGhost();
		SpawnSmoke();
	}

	private void SpawnGhost()
	{
		var ghost = ghostScene.Instantiate<dashGhost>();
		GetParent().AddChild(ghost);

		ghost.GlobalPosition = GlobalPosition;
		ghost.FlipH = _anim.FlipH;

		var frames = _anim.SpriteFrames;
		var currentAnimation = _anim.Animation;
		var currentFrame = _anim.Frame;
		var frameTexture = frames.GetFrameTexture(currentAnimation, currentFrame);

		Image image = frameTexture.GetImage();
		ImageTexture frozenTexture = ImageTexture.CreateFromImage(image);

		ghost.Texture = frozenTexture;
		
	}
	
	private void SpawnSmoke() {
		var smoke = dashSmoke.Instantiate<DashSmoke>();
		GetParent().AddChild(smoke);
		smoke.GlobalPosition = GlobalPosition;
	}
	
	private void OnTakeDamage() {
		_flashMaterial.SetShaderParameter("flash_red", false);
	}
}
