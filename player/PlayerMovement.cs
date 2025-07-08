using Godot;

public partial class PlayerMovement : CharacterBody2D
{
	public override void _EnterTree()
	{
		SetMultiplayerAuthority(int.Parse(Name));
	}

	public override void _Process(double delta)
	{
		if (IsMultiplayerAuthority())
		{
			Velocity = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down") * 400;
		}
		
		MoveAndSlide();
	}
}
