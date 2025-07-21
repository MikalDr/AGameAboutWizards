using Godot;

public partial class MagicOrb : Spell
{
  [Export] private double _audioLength;
  private double _time;
  protected override void Setup()
  {
    base.Setup();
    IsEffectOver = delta =>
    {
      _time += delta;
      return _audioLength <= _time;
    };
  }

  protected override void OnHurtboxEnter(Area2D area)
  {
    GD.Print("I hit");
    if (HasHit) return;
    SpellAudio.StreamPaused = false;
    SafelyDequeSelf();
  }
}