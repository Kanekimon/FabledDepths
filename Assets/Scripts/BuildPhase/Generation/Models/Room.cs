using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

<<<<<<< Updated upstream:Assets/Scripts/Building/Generation/Models/Room.cs
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
=======
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


    //public Vector3 Center => new Vector3(MinX + (Width / 2), MinY + (Height / 2), 0);

    public (Vector2 XminYmin, Vector2 XmaxYmin, Vector2 XminYmax, Vector2 XmaxYmax) Center =>
       (
        new Vector2(Position.x + ((Width - 1) / 2), Position.y + ((Height - 1) / 2)),
        new Vector2(Position.x + ((Width - 1) / 2) + 1, Position.y + ((Height - 1) / 2)),
        new Vector2(Position.x + ((Width - 1) / 2), Position.y + ((Height - 1) / 2) + 1),
        new Vector2(Position.x + ((Width - 1) / 2) + 1, Position.y + ((Height - 1) / 2) + 1)
       );

    public (Vector2 XminYmin, Vector2 XmaxYmin, Vector2 XminYmax, Vector2 XmaxYmax) LocalCenter =>
    (
        new Vector2(((Width - 1) / 2), ((Height - 1) / 2)),
        new Vector2(((Width - 1) / 2) + 1, ((Height - 1) / 2)),
        new Vector2(((Width - 1) / 2), ((Height - 1) / 2) + 1),
        new Vector2(((Width - 1) / 2) + 1, ((Height - 1) / 2) + 1)
    );


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
        Height = height;
        Width = width;

        Position = position;

    }
}

[Serializable]

public class Room : BaseRoom
{
    public Guid Id { get; private set; }
    private List<Tile> _tiles;
    public DoorPlacement Doors = DoorPlacement.none;


    public BoundingBox BoundingBox { get; private set; }

    public List<Tile> Tiles { get { return _tiles; } }

>>>>>>> Stashed changes:Assets/Scripts/BuildPhase/Generation/Models/Room.cs

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
<<<<<<< Updated upstream:Assets/Scripts/Building/Generation/Models/Room.cs

    public Room((int XIndex, int YIndex) index)
=======
    public void Init((int X, int Y) index, int width, int height)
>>>>>>> Stashed changes:Assets/Scripts/BuildPhase/Generation/Models/Room.cs
    {
        _tiles = new List<Tile>();
        Id = new Guid();
        Index = index;

        RoomObject = this.gameObject;
        RoomObject.name = $"Room [{Index.X}|{Index.Y}]";
        RoomObject.tag = "Room";
<<<<<<< Updated upstream:Assets/Scripts/Building/Generation/Models/Room.cs
=======
        RoomObject.transform.position = new Vector3((index.X * width), (index.Y * height), 0);

        BoundingBox = new BoundingBox(RoomObject.transform.position, width, height);
        if (Index == (0, 0))
        {
            Destroy(RoomObject.GetComponent<BoxCollider2D>());
        }
        else
        {
            BoxCollider2D collider = RoomObject.GetComponent<BoxCollider2D>();
            collider.size = new Vector2(width, height);
            collider.offset = new Vector2(width / 2, height / 2);
        }

>>>>>>> Stashed changes:Assets/Scripts/BuildPhase/Generation/Models/Room.cs
    }

    public void ChangeIndex((int X, int Y) Index)
    {
        this.Index = Index;
        RoomObject.name = $"Room [{Index.X}|{Index.Y}]";

    }


    public void Destroy()
    {
        foreach(Tile t in _tiles)
        {
            GameObject.DestroyImmediate(t.TileObject);
        }
        GameObject.DestroyImmediate(RoomObject);
    }
}


