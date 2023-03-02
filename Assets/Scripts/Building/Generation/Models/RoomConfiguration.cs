using System;


[Serializable]


[Flags]
public enum DoorPlacement
{
    none = 0,
    north = 1,
    east = 2,
    south = 4,
    west = 8
}

[Serializable]
public class RoomConfiguration
{
    public RoomType RoomType;
    public float ObstaclePropability;
    public int ObastacleSeedPoints;
    public DoorPlacement DoorPlacement;
    public int Width;
    public int Height;
}

