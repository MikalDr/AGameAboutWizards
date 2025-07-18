using GdUnit4;
using Godot;

namespace AGameAboutWizards.tests;

[TestSuite]
public class SettingsServiceTest
{
  [TestCase]
  [RequireGodotRuntime]
  public void SettingMapsToInputActions()
  {
    foreach (var setting in SettingsService.GetSettingEnums())
    {
      var strSetting = SettingsService.GetStrValue(setting);
      Assertions.AssertString(strSetting).IsNotEmpty();
      Assertions.AssertBool(InputMap.GetActions().Contains(strSetting)).IsTrue();
    }
  }
}
