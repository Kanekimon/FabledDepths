using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Room
{
    public Guid Id { get; private set; }
    private List<Tile> _tiles;
    private GameObject _roomObject;
    private int _xIndex;
    private int _yIndex;
    
    public (int X, int Y) Index
    {
        get { return (_xIndex, _yIndex); }
        set { _xIndex = value.X; _yIndex = value.Y; }
    }


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
            if(_tiles.Any(t => t.X == x && t.Y == y))
            {
                this[x, y] = value;
            }
            else
            {
                _tiles.Add(value);
            }
        }
    }

    public Room((int XIndex, int YIndex) index)
    {
        _tiles = new List<Tile>();
        Id = new Guid();
        Index = index;
        RoomObject = new GameObject();
        RoomObject.name = $"Room [{Index.X}|{Index.Y}]";
        RoomObject.tag = "Room";
    }

    public void Destroy()
    {
        foreach(Tile t in _tiles)
        {
            GameObject.DestroyImmediate(t.TileObject);
        }
        _tiles.Clear();
        GameObject.DestroyImmediate(_roomObject);
        _roomObject = new GameObject();
    }
}


