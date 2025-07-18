using System;
using Godot;

public partial class Keybind : HBoxContainer
{
  private RichTextLabel _label;
  private Button _triggerRemap;
  private bool _isListening;
  private Setting _setting;

  public override void _Ready()
  {
    _label = GetNode<RichTextLabel>("ButtonIcon");
    _triggerRemap = GetNode<Button>("TriggerRemap");
    if (_label == null) throw new Exception("Missing ButtonIcon");
    if (_triggerRemap == null) throw new Exception("Missing TriggerRemap");
  }

  public override void _Input(InputEvent @event)
  {
    if (!_isListening) return;
    if (@event is InputEventMouseMotion) return;
    _isListening = false;
    GD.Print("Remapped: " + _setting + ": " + @event.AsText());
    SettingsService.Remap(_setting, @event);
    _triggerRemap.Text = "Key: " + SettingsService.GetCurrentKeymap(_setting).Join();
  }

  public void SetRemap(Setting setting)
  {
    _setting = setting;
    _label.Text = SettingsService.GetStrValue(setting);
    _triggerRemap.Text = "Key: " + SettingsService.GetCurrentKeymap(setting).Join();
    _triggerRemap.ButtonUp += () =>
    { 
      GD.Print("Button pressed: " + _label.Text);
      _isListening = true;
    };
  }
}
