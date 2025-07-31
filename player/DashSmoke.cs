using Godot;
using System;

public partial class DashSmoke : AnimatedSprite2D
{
	public override void _Ready()
	{
		AnimationFinished += OnAnimationFinished;
		Play("smoke");
	}
	
	private void OnAnimationFinished()
	{
		QueueFree();
	}
}
