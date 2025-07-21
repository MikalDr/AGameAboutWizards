using System;
using System.Linq;
using Godot;

public partial class VolumeService : Node
{
  public static double Sound { get; private set; } = 0.5f;
  public static double Music { get; private set; } = 0.5f;

  public static Volume[] GetVolumes()
  {
    return Enum.GetNames<Volume>().Select(Enum.Parse<Volume>).ToArray();
  }

  public static string GetVolumeStr(Volume volume)
  {
    return volume switch
    {
      Volume.Music => "Music",
      Volume.Sound => "Sound",
      _ => throw new ArgumentOutOfRangeException(nameof(volume), volume, null)
    };
  }

  public static double GetStaringVolumeStr(Volume volume)
  {
    return volume switch
    {
      Volume.Music => Music,
      Volume.Sound => Sound,
      _ => throw new ArgumentOutOfRangeException(nameof(volume), volume, null)
    };
  }

  public static Action<double> GetVolumeAction(Volume volume)
  {
    return volume switch
    {
      Volume.Music => v => Music = v,
      Volume.Sound => v => Sound = v,
      _ => throw new ArgumentOutOfRangeException(nameof(volume), volume, null)
    };
  }
}

public enum Volume
{
  Music,
  Sound
}