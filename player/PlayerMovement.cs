using Godot;

public partial class PlayerMovement : CharacterBody2D
{
	[Export]
	private float MaxSpeed = 400f;
	[Export]
	private float Acceleration = 3000f;

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

			if (inputDirection != Vector2.Zero)
			{
				inputDirection = inputDirection.Normalized();
				Velocity = Velocity.MoveToward(inputDirection * MaxSpeed, Acceleration * (float)delta);

				_anim.Animation = "run";

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
