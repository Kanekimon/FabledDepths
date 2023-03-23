using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class LayerGenerator : IGenerator
{
    public int[] XMask = new int[] { -1, 0, 1, 1, 1, 0, -1, -1 };
    public int[] YMask = new int[] { 1, 1, 1, 0, -1, -1, -1, 0 };


    public Room GenerateRoom((int X,int Y) Index, RoomConfiguration rC, int width, int height, GameObject container = null, bool generateGameObject = true)
    {

        Room r = new Room();
        r.Init(Index, width, height, generateGameObject);
        r.Doors = rC.DoorPlacement;
        GenerateTiles(r, rC, r.RoomObject != null ? r.RoomObject.transform : null);
        if (generateGameObject)
        {
            if (container != null)
                r.RoomObject.transform.parent = container.transform;
        }


           
        return r;
    }

    void GenerateTiles(Room r, RoomConfiguration rC, Transform parent = null)
    {
        CreateBaseTileMap(ref r, rC, parent);
        CreateDoors(ref r, rC, parent);
        CreateObstacles(ref r, rC, parent);
    }


    void CreateBaseTileMap(ref Room r, RoomConfiguration rC, Transform parent = null)
    {
        for (int x = 0; x < r.BoundingBox.Width; x++)
        {
            for (int y = 0; y < r.BoundingBox.Height; y++)
            {
                if (r[x, y] == null)
                {
                    r[x, y] = GenerateTile(x, y, r.BoundingBox, r, parent);
                }
            }
        }
    }

    private void CreateObstacles(ref Room r, RoomConfiguration rC, Transform parent = null)
    {       
        Stack<Vector3> spawnPoints = new Stack<Vector3>();
        List<Vector3> closed = new List<Vector3>();
        for(int i = 0; i < rC.ObastacleSeedPoints; i++)
        {
            Vector3 seedPoint = r.BoundingBox.GetPoint();
            if (!spawnPoints.Contains(seedPoint))
                spawnPoints.Push(seedPoint);
        }


        float prob = rC.ObstaclePropability;

        while (spawnPoints.Count > 0)
        {
            Vector3 spawnPoint = spawnPoints.Pop();

            closed.Add(spawnPoint);
            for(int i = 0; i < XMask.Length; i++)
            {
                Vector3 newPoint = new Vector3(spawnPoint.x + XMask[i], spawnPoint.y + YMask[i], 0);
                if (!spawnPoints.Contains(newPoint) && !closed.Contains(newPoint) && r.BoundingBox.IsPointValid(newPoint) && UnityEngine.Random.Range(0f, 1f) < prob)
                {
                    prob -= 0.01f;
                    spawnPoints.Push(newPoint);
                }
            }


            if (r[(int)spawnPoint.x, (int)spawnPoint.y].TileType == TileType.edge || UnityEngine.Random.Range(0f, 1f) < 0.7f)
                continue;



            GameObject obstacle = GameObject.Instantiate(Resources.Load<GameObject>("Room/Test_Obstacle"));
            obstacle.transform.parent = r[(int)spawnPoint.x, (int)spawnPoint.y].TileObject.transform;
            obstacle.transform.localPosition = new Vector3(0, 0, 0);
            obstacle.GetComponent<SpriteRenderer>().sortingOrder = 1;

        }
    }



    void CreateDoors(ref Room r, RoomConfiguration rC, Transform parent = null)
    {
        (Vector2 XminYmin, Vector2 XmaxYmin, Vector2 XminYmax, Vector2 XmaxYmax) c = r.BoundingBox.LocalCenter;


        if (rC.DoorPlacement.HasFlag(DoorPlacement.north))
        {
            r[(int)c.XminYmin.x, (int)r.BoundingBox.Height - 1] = new Tile((int)c.XminYmin.x, (int)r.BoundingBox.Height-1, TileType.door, DoorPlacement.north, parent);
            r[(int)c.XmaxYmin.x, (int)r.BoundingBox.Height - 1] = new Tile((int)c.XmaxYmin.x, (int)r.BoundingBox.Height-1, TileType.door, DoorPlacement.north, parent);
        }
        if (rC.DoorPlacement.HasFlag(DoorPlacement.east))
        {
            r[(int)r.BoundingBox.Width - 1, (int)c.XminYmin.y] = new Tile((int)r.BoundingBox.Width-1, (int)c.XminYmin.y, TileType.door, DoorPlacement.east, parent);
            r[(int)r.BoundingBox.Width - 1, (int)c.XminYmax.y] = new Tile((int)r.BoundingBox.Width-1, (int)c.XminYmax.y, TileType.door, DoorPlacement.east, parent);
        }
        if (rC.DoorPlacement.HasFlag(DoorPlacement.south))
        {
            r[(int)c.XminYmin.x, 0] = new Tile((int)c.XminYmin.x, 0, TileType.door, DoorPlacement.north, parent) ;
            r[(int)c.XmaxYmin.x, 0] = new Tile((int)c.XmaxYmin.x, 0, TileType.door, DoorPlacement.north, parent);
        }
        if (rC.DoorPlacement.HasFlag(DoorPlacement.west))
        {
            r[0, (int)c.XminYmin.y] = new Tile(0, (int)c.XminYmin.y, TileType.door, DoorPlacement.west, parent);
            r[0, (int)c.XminYmax.y] = new Tile(0, (int)c.XminYmax.y, TileType.door, DoorPlacement.west, parent);
        }
        r.Doors = rC.DoorPlacement;

    }


    Tile GenerateTile(int x, int y, BoundingBox bb, Room r, Transform parent = null)
    {
        int xOffset = r.Index.X * bb.Width;
        int yOffset = r.Index.Y * bb.Height;


        if (x == bb.MinX - xOffset || x == bb.MaxX - xOffset || y == bb.MinY - yOffset || y == bb.MaxY - yOffset)
        {
            return new Tile(x, y, TileType.edge, DoorPlacement.none, parent);
        }
        else
            return new Tile(x, y, TileType.normal, DoorPlacement.none, parent);

    }

}

