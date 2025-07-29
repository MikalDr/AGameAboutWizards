using Godot;
using System;

public partial class PlayerMovement : CharacterBody2D
{
	[Export]
	private float MaxSpeed = 400f;
	[Export]
	private float DashSpeed = 800;
	[Export]
	private float DashDuration = 0.2f;
	[Export]
	private float DashCooldown = 2f;
	[Export]
	private float Acceleration = 5000f;
	[Export]
	private float DamageTime = 0.1f;
	
	private bool isDashing = false;
	
	private int Health = 100;
	
	private Timer dashCooldownTimer;
	private Timer dashDurationTimer;
	private Timer damageTimer;
	
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
		dashCooldownTimer = GetNode<Timer>("DashCooldown");
		dashDurationTimer = GetNode<Timer>("DashDuration");
		damageTimer = new Timer();
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
			if (inputDirection != Vector2.Zero)
			{
				inputDirection = inputDirection.Normalized();
				
				if ((Input.IsActionJustPressed("dash") && dashCooldownTimer.IsStopped()))
				{
					dashCooldownTimer.Start(DashCooldown);
					dashDurationTimer.Start(DashDuration);
					Velocity = DashSpeed * inputDirection;
					isDashing = true;
				} 
				else if (isDashing)
				{
					_flashMaterial.SetShaderParameter("flash_white", true);
					if (dashDurationTimer.IsStopped())
					{
						isDashing = false; 
					}
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
				Velocity = Vector2.Zero;
				_anim.Stop();
				_anim.Animation = "idle";
			}

		}
		
		MoveAndSlide();
	}
		
	private void OnDashDurationTimeout() {
		
	}
	private void OnDashCooldownTimeout() {
		
	}
	private void OnTakeDamage() {
		_flashMaterial.SetShaderParameter("flash_red", false);
	}
}
