using Godot;
using System;

public partial class PlayerMovement : CharacterBody2D
{
	[Export]
	private float MaxSpeed = 400f;
	[Export]
	private float DashSpeed = 800;
	[Export]
	private float DashDuration = 0.1f;
	[Export]
	private float DashCooldown = 2f;
	[Export]
	private float Acceleration = 5000f;
	
	private bool isDashing = false;
	
	private int Health = 100;
	
	private Timer dashCooldownTimer;
	private Timer dashDurationTimer;
	
	private AnimatedSprite2D _anim;

	public void takeDamage(int amount)
	{
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
		
		// Dash Timers
		dashCooldownTimer = GetNode<Timer>("DashCooldown");
		dashDurationTimer = GetNode<Timer>("DashDuration");
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
				//Velocity = Velocity.MoveToward(inputDirection * MaxSpeed, Acceleration * (float)delta);
				
				if ((Input.IsActionJustPressed("dash") && dashCooldownTimer.IsStopped()))
				{
					dashCooldownTimer.Start(DashCooldown);
					dashDurationTimer.Start(DashDuration);
					Velocity = DashSpeed * inputDirection;
					_anim.
					isDashing = true;
				} 
				else if (isDashing)
				{
					if (dashDurationTimer.IsStopped())
					{
						isDashing = false; 
					}
				}
				 else {
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
}
