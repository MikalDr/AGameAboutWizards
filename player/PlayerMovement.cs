using Godot;

public partial class PlayerMovement : CharacterBody2D
{
	[Export]
	private float MaxSpeed = 400f;
	[Export]
	private float Acceleration = 5000f;

	private AnimatedSprite2D _anim;

	public override void _EnterTree()
	{
		SetMultiplayerAuthority(int.Parse(Name));
	}

	public override void _Ready()
	{
		_anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_anim.Animation = "idle";
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
				Velocity = Velocity.MoveToward(inputDirection * MaxSpeed, Acceleration * (float)delta);
				
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
}
