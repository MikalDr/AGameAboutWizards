using System;

public enum Direction
{
  North,
  South,
  East,
  West,
  NorthWest,
  NorthEast,
  SouthEast,
  SouthWest,
}

public static class DirectionUtils
{
  public static double GetRadians(this Direction direction)
  {
    return direction switch
    {
      Direction.North => 0, // 0
      Direction.South => Math.PI, // 180
      Direction.East => Math.PI / 2, // 90
      Direction.West => 3 * Math.PI / 2, // 270
      Direction.NorthWest => 7 * Math.PI / 4, // 315
      Direction.NorthEast => Math.PI / 4, // 45
      Direction.SouthEast => 3 * Math.PI / 4,
      Direction.SouthWest => 5 * Math.PI / 4,
      _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
    };
  }
}