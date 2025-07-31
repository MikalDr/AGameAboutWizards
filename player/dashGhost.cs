using Godot;
using System;

public partial class dashGhost : Sprite2D
{
	public override void _Ready()
	{
		Modulate = new Color(1, 1, 1, 0.5f);
		
		var tween = GetTree().CreateTween();
		tween.TweenMethod(Callable.From<float>(SetAlpha), 0.5f, 0.0f, .3f)
			 .SetTrans(Tween.TransitionType.Sine)
			 .SetEase(Tween.EaseType.In);
		tween.TweenCallback(Callable.From(QueueFree));
	}
	
	private void SetAlpha(float alpha)
	{
		var color = Modulate;
		color.A = alpha;
		Modulate = color;
	}
}
