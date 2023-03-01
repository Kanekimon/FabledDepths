using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[Serializable]
public class BoundingBox
{
    public Vector3 Position;
    public int Height;
    public int Width;
    public int MaxX => Width + (int)Position.x -1;
    public int MaxY => Height + (int)Position.y-1;

    public int MinX => (int)Position.x;
    public int MinY => (int)Position.y;

    public Vector3 Center => new Vector3(MinX + (Width / 2), MinY + (Height/2), 0);


    public Vector3 GetPoint()
    {
        int x = UnityEngine.Random.Range(MinX + 1, MaxX);
        int y = UnityEngine.Random.Range(MinY + 1, MaxY);
        return new Vector3(x,y,0);
    }

    public bool IsPointValid(Vector3 point)
    {
        return point.x >= MinX && point.x <= MaxX && point.y >= MinY && point.y <= MaxY;
    }
}

[Serializable]
public class RoomConfiguration
{
    public BoundingBox BoundingBox;
    public RoomType RoomType;
    public float ObstaclePropability;
    public int ObastacleSeedPoints;
}

