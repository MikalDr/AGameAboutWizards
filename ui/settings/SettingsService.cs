using Godot;
using System;
using System.Linq;

public partial class SettingsService : Node
{
  public static void Remap(Setting setting, InputEvent keypress)
  {
    var str = GetStrValue(setting);
    foreach (var oldEvent in InputMap.ActionGetEvents(GetStrValue(setting)))
    {
      InputMap.ActionEraseEvent(str, oldEvent);
    }

    InputMap.ActionAddEvent(str, keypress);
  }

  public static Setting[] GetSettingEnums()
  {
    return Enum.GetNames<Setting>().Select(Enum.Parse<Setting>).ToArray();
  }

  public static string GetStrValue(Setting setting)
  {
    return setting switch
    {
      Setting.Forward => "_forward",
      Setting.Backward => "_backward",
      Setting.Left => "_left",
      Setting.Right => "_right",
      Setting.Use => "_use",
      _ => throw new ArgumentOutOfRangeException(nameof(setting), setting, null)
    };
  }

  public static string[] GetCurrentKeymap(Setting setting)
  {
    return InputMap.ActionGetEvents(GetStrValue(setting))
      .Where(i => i is InputEventKey)
      .Select(i => ((InputEventKey) i).AsText().ToLower().Replace("(physical)", "").Trim()).ToArray();
  }
}
