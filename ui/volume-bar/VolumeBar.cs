using Godot;
using System;
using System.Globalization;

public partial class VolumeBar : HBoxContainer
{
  [Export] private Label _volumeLabel;
  [Export] private HScrollBar _volumeBar;
  [Export] private Label _volumeValue;
  public double Value { get; private set; }

  public override void _Ready()
  {
    if (_volumeLabel == null) throw new Exception("VolumeLabel not initialized");
    if (_volumeBar == null) throw new Exception("VolumeBar not initialized");
    _volumeBar.MinValue = 0;
    _volumeBar.Step = 0.01;
    _volumeBar.MaxValue = 1;
    if (_volumeValue == null) throw new Exception("VolumeValue not initialized");
  }

  public void AddChangeAction(Action<double> action)
  {
    _volumeBar.Scrolling += () =>
    {
      Value = _volumeBar.Value;
      _volumeValue.Text = Math.Truncate(Value * 100).ToString(CultureInfo.InvariantCulture) + "%";
      action(Value);
    };
  }

  public void SetStartingVolumeLabel(double value)
  {
    _volumeBar.Value = value;
    Value = value;
    _volumeValue.Text = Math.Truncate(value * 100).ToString(CultureInfo.InvariantCulture) + "%";
  }

  public void SetLabel(string label)
  {
    _volumeLabel.Text = label;
  }
}
