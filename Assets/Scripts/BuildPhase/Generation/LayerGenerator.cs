using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class LayerGenerator : IGenerator
{
    public int[] XMask = new int[] { -1, 0, 1, 1, 1, 0, -1, -1 };
    public int[] YMask = new int[] { 1, 1, 1, 0, -1, -1, -1, 0 };

    public Room GenerateRoom((int X,int Y) Index, RoomConfiguration rC, int width, int height, GameObject container = null)
    {
<<<<<<< Updated upstream:Assets/Scripts/Building/Generation/LayerGenerator.cs
        Room r = new Room((0, 0));
        CreateBaseTileMap(ref r, rC);
=======
        GameObject room = GameObject.Instantiate(Resources.Load("Room/BaseRoom")) as GameObject;
        if(container != null)
            room.transform.parent = container.transform;

        Room r = room.GetComponent<Room>();  //Room((0, 0), rC.Width, rC.Height);
        r.Init(Index, width,height);
        CreateBaseTileMap(ref r);
>>>>>>> Stashed changes:Assets/Scripts/BuildPhase/Generation/LayerGenerator.cs
        CreateObstacles(ref r, rC);
        //CreateMonsterSpawns();
        return r;
    }


    void CreateBaseTileMap(ref Room r, RoomConfiguration rC)
    {
        for (int x = 0; x < rC.BoundingBox.Width; x++)
        {
            for (int y = 0; y < rC.BoundingBox.Height; y++)
            {
                if (r[x, y] == null)
                {
                    r[x, y] = GenerateTile(x, y, rC.BoundingBox, r);
                }
            }
        }
    }

    private void CreateObstacles(ref Room r, RoomConfiguration rC)
    {       
        Stack<Vector3> spawnPoints = new Stack<Vector3>();
        List<Vector3> closed = new List<Vector3>();
        for(int i = 0; i < rC.ObastacleSeedPoints; i++)
        {
            Vector3 seedPoint = rC.BoundingBox.GetPoint();
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
                if (!spawnPoints.Contains(newPoint) && !closed.Contains(newPoint) && rC.BoundingBox.IsPointValid(newPoint) && UnityEngine.Random.Range(0f, 1f) < prob)
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


<<<<<<< Updated upstream:Assets/Scripts/Building/Generation/LayerGenerator.cs
=======
    void CreateDoors(ref Room r, RoomConfiguration rC)
    {
        (Vector2 XminYmin, Vector2 XmaxYmin, Vector2 XminYmax, Vector2 XmaxYmax) c = r.BoundingBox.LocalCenter;


        if (rC.DoorPlacement.HasFlag(DoorPlacement.north))
        {
            r[(int)c.XminYmin.x, (int)r.BoundingBox.Height - 1] = new Tile((int)c.XminYmin.x, (int)r.BoundingBox.Height-1, r.RoomObject.transform, TileType.door, DoorPlacement.north);
            r[(int)c.XmaxYmin.x, (int)r.BoundingBox.Height - 1] = new Tile((int)c.XmaxYmin.x, (int)r.BoundingBox.Height-1, r.RoomObject.transform, TileType.door, DoorPlacement.north);
        }
        if (rC.DoorPlacement.HasFlag(DoorPlacement.east))
        {
            r[(int)r.BoundingBox.Width - 1, (int)c.XminYmin.y] = new Tile((int)r.BoundingBox.Width-1, (int)c.XminYmin.y,  r.RoomObject.transform, TileType.door, DoorPlacement.east);
            r[(int)r.BoundingBox.Width - 1, (int)c.XminYmax.y] = new Tile((int)r.BoundingBox.Width-1, (int)c.XminYmax.y, r.RoomObject.transform, TileType.door, DoorPlacement.east);
        }
        if (rC.DoorPlacement.HasFlag(DoorPlacement.south))
        {
            r[(int)c.XminYmin.x, 0] = new Tile((int)c.XminYmin.x, 0, r.RoomObject.transform, TileType.door, DoorPlacement.north) ;
            r[(int)c.XmaxYmin.x, 0] = new Tile((int)c.XmaxYmin.x, 0, r.RoomObject.transform, TileType.door, DoorPlacement.north);
        }
        if (rC.DoorPlacement.HasFlag(DoorPlacement.west))
        {
            r[0, (int)c.XminYmin.y] = new Tile(0, (int)c.XminYmin.y, r.RoomObject.transform, TileType.door, DoorPlacement.west);
            r[0, (int)c.XminYmax.y] = new Tile(0, (int)c.XminYmax.y, r.RoomObject.transform, TileType.door, DoorPlacement.west);
        }
        r.Doors = rC.DoorPlacement;

    }


>>>>>>> Stashed changes:Assets/Scripts/BuildPhase/Generation/LayerGenerator.cs

    Tile GenerateTile(int x, int y, BoundingBox bb, Room r)
    {
        int xOffset = r.Index.X * bb.Width;
        int yOffset = r.Index.Y * bb.Height;


        if (x == bb.MinX - xOffset || x == bb.MaxX - xOffset || y == bb.MinY - yOffset || y == bb.MaxY - yOffset)
        {
            return new Tile(x, y, r.RoomObject.transform, TileType.edge);
        }
        else
            return new Tile(x, y, r.RoomObject.transform);

    }

}

