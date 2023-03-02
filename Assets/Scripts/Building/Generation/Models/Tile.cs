using Newtonsoft.Json;
using System;
using UnityEngine;

public enum TileType
{
    edge,
    door,
    normal
}

[Serializable]
public class Tile
{
    private float _x;
    private float _y;

     private GameObject _tileObject;
    private TileType _tileType;

    public DoorPlacement Placement { get; set; }

    public float X
    {
        get { return _x; }
        set { _x = value; }
    }
    public float Y
    {
        get { return _y; }
        set { _y = value; }
    }

    [JsonIgnore]
    public GameObject TileObject
    {
        get { return _tileObject; }
        set { _tileObject = value; }
    }

    public TileType TileType
    {
        get { return _tileType; }
        set { _tileType = value; }
    }


    public Tile(int x, int y, Transform parent, TileType tileType = TileType.normal, DoorPlacement doorPlacement = DoorPlacement.none)
    {
        _x = x;
        _y = y;
        _tileType = tileType;
        Placement = doorPlacement;

        if (TileType == TileType.normal)
            _tileObject = GameObject.Instantiate(Resources.Load<GameObject>("Room/Test_Tile"));
        else if (TileType == TileType.edge)
            _tileObject = GameObject.Instantiate(Resources.Load<GameObject>("Room/Test_Wall"));
        else if (TileType == TileType.door)
        {
            _tileObject = GameObject.Instantiate(Resources.Load<GameObject>("Room/Test_Door"));
            RotateDoor(doorPlacement);
        }

        _tileObject.transform.parent = parent;
        _tileObject.transform.localPosition = new Vector3(_x, _y, 0);
        _tileObject.name = $"({TileType.ToString()}) Tile [{_x},{_y}]";
    }

    void RotateDoor(DoorPlacement doorPlacement)
    {
        if (doorPlacement.HasFlag(DoorPlacement.west) || doorPlacement.HasFlag(DoorPlacement.east))
        {
            float zRotation = 90.0f;
            _tileObject.transform.eulerAngles = new Vector3(_tileObject.transform.eulerAngles.x, _tileObject.transform.eulerAngles.y, zRotation);
        }
    }

}

