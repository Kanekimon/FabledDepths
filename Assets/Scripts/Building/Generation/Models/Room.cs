using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BoundingBox
{
    public Vector3 Position;
    public int Height;
    public int Width;
    public int MaxX => Width + (int)Position.x - 1;
    public int MaxY => Height + (int)Position.y - 1;

    public int MinX => (int)Position.x;
    public int MinY => (int)Position.y;

    public Vector3 Center => new Vector3(MinX + (Width / 2), MinY + (Height / 2), 0);


    public Vector3 GetPoint()
    {
        int x = UnityEngine.Random.Range(MinX + 1, MaxX);
        int y = UnityEngine.Random.Range(MinY + 1, MaxY);
        return new Vector3(x, y, 0);
    }

    public bool IsPointValid(Vector3 point)
    {
        return point.x >= MinX && point.x <= MaxX && point.y >= MinY && point.y <= MaxY;
    }

    public BoundingBox(Vector3 position, int width, int height)
    {
        Position = position;
        Height = height;
        Width = width;
    }
}

[Serializable]

public class Room 
{
    public Guid Id { get; private set; }
     private List<Tile> _tiles;
    [JsonIgnore] private GameObject _roomObject;
    private int _xIndex;
    private int _yIndex;

    public BoundingBox BoundingBox { get; private set; }

    public List<Tile> Tiles { get { return _tiles; } }

    public (int X, int Y) Index
    {
        get { return (_xIndex, _yIndex); }
        set { _xIndex = value.X; _yIndex = value.Y; }
    }

    [JsonIgnore]
    public GameObject RoomObject
    {
        get { return _roomObject; }
        private set { _roomObject = value; }
    }

    public Tile this[int x, int y]
    {
        get { return _tiles.FirstOrDefault(t => t.X == x && t.Y == y); }
        set
        {
            if (_tiles.Any(t => t.X == x && t.Y == y))
            {
                GameObject.DestroyImmediate(this[x, y].TileObject);
                _tiles.Remove(_tiles.Where(t => t.X == x && t.Y == y).FirstOrDefault());
                _tiles.Add(value);
            }
            else
            {
                _tiles.Add(value);
            }
        }
    }

    public Room((int XIndex, int YIndex) index, int width, int height)
    {
        _tiles = new List<Tile>();
        Id = new Guid();
        Index = index;
        RoomObject = new GameObject();
        RoomObject.name = $"Room [{Index.X}|{Index.Y}]";
        RoomObject.tag = "Room";
        BoundingBox = new BoundingBox(new Vector3(index.XIndex, index.YIndex, 0), width, height);
    }

    public void Destroy()
    {
        foreach (Tile t in _tiles)
        {
            GameObject.DestroyImmediate(t.TileObject);
        }
        _tiles.Clear();
        GameObject.DestroyImmediate(_roomObject);
        _roomObject = new GameObject();
    }


}


