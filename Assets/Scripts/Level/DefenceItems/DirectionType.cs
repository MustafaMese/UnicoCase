using System;

[Flags]
public enum DirectionType
{
    None = 1 << 0,
    Forward = 1 << 1,
    Backward = 1 << 2,
    Left = 1 << 3,
    Right = 1 << 4,
}