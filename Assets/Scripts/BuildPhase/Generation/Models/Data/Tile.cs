using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum TileType
{
    edge,
    door, 
    path,
    normal
}

public class Tile
{
    private float _x;
    private float _y;
    private GameObject _tileObject;
    private TileType _tileType;
    private DoorPlacement _doorPlacement;


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

    public DoorPlacement Placement
    {
        get { return _doorPlacement; }
        set { _doorPlacement = value; }
    }


    public Tile(int x, int y, TileType tileType = TileType.normal, DoorPlacement doorPlacement = DoorPlacement.none, Transform parent = null)
    {
        _x = x;
        _y = y;
        _tileType = tileType;
        _doorPlacement = doorPlacement;


        if (parent != null)
        {
            InstantiateTile(parent);
        }
    }

    public void InstantiateTile(Transform parent)
    {
        if (TileType == TileType.normal)
            _tileObject = GameObject.Instantiate(Resources.Load<GameObject>("Room/Test_Tile"));
        else if (TileType == TileType.edge)
            _tileObject = GameObject.Instantiate(Resources.Load<GameObject>("Room/Test_Wall"));
        else if(TileType == TileType.path)
        {
            _tileObject = GameObject.Instantiate(Resources.Load<GameObject>("Room/Test_Path"));
        }
        else if (TileType == TileType.door)
        {
            _tileObject = GameObject.Instantiate(Resources.Load<GameObject>("Room/Test_Door"));

            if (_doorPlacement.HasFlag(DoorPlacement.west | DoorPlacement.east)) RotateDoor();
        }

        _tileObject.transform.parent = parent;
        _tileObject.transform.localPosition = new Vector3(_x, _y, 0);
        _tileObject.name = $"Tile({_tileType}) [{_x},{_y}]";
    }


    void RotateDoor()
    {
        float zRotation = 90.0f;
        _tileObject.transform.eulerAngles = new Vector3(_tileObject.transform.eulerAngles.x, _tileObject.transform.eulerAngles.y, zRotation);
    }

}

