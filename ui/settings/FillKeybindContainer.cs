using Godot;
using System;
using Godot.Collections;

public partial class FillKeybindContainer : VBoxContainer
{
  [Export] private PackedScene _keybindContainer;
  [Export] private PackedScene _volumeBar;
  public override void _Ready()
  {
    Array<Node> children = [];
    foreach (var child in GetChildren()) RemoveChild(child);
    
    children.AddRange(VolumeBarSetup());
    children.AddRange(KeybindSetup());
    children.AddRange(GetChildren());
    
    foreach (var child in children) AddChild(child);
  }

  private Array<Node> VolumeBarSetup()
  {
    if (_volumeBar == null) throw new Exception("FillKeybindContainer not initialized");
    Array<Node> children = [];

    foreach (var volume in VolumeService.GetVolumes())
    {
      var txt = VolumeService.GetVolumeStr(volume);
      var bar = _volumeBar.Instantiate();
      if (bar is not HBoxContainer) throw new Exception("VolumeBar is not HBoxContainer");
      var controller = bar.GetNode<VolumeBar>(".");
      if (controller == null) throw new Exception("Could not find Keybind script on VolumeBar");
      bar._Ready();
      controller.SetLabel(txt);
      controller.AddChangeAction(VolumeService.GetVolumeAction(volume));
      controller.SetStartingVolumeLabel(VolumeService.GetStaringVolumeStr(volume));
      children.Add(bar);
    }
    
    return children;
  }

  private Array<Node> KeybindSetup()
  {
    if (_keybindContainer == null) throw new Exception("FillKeybindContainer not initialized");
    Array<Node> children = [];
    foreach (var setting in SettingsService.GetSettingEnums())
    {
      var box = _keybindContainer.Instantiate();
      if (box is not HBoxContainer) throw new Exception("FillKeybindContainer is not HBoxContainer");
      var keybind = box.GetNode<Keybind>(".");
      if (keybind == null) throw new Exception("Could not find Keybind script on FillKeyBindContainer");
      keybind._Ready();
      keybind.SetRemap(setting);
      children.Add(keybind);
    }
    return children;
  }
}
