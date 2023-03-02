using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

internal class Seedpoint
{
    float prob;
    public Vector3 Position;
    public Seedpoint(float probability, Vector3 position)
    {
        prob = probability;
        Position = position;
    }

    public static Seedpoint GenerateSeed(Seedpoint s, Vector3 position)
    {
        return new Seedpoint((s.prob * 0.7f), position);
    }

    public int X => (int)Position.x;
    public int Y => (int)Position.y;
    public float Prob => prob;
}




public class LayerGenerator : IGenerator
{
    public int[] XMask = new int[] { -1, 0, 1, 1, 1, 0, -1, -1 };
    public int[] YMask = new int[] { 1, 1, 1, 0, -1, -1, -1, 0 };

    public Room GenerateRoom(RoomConfiguration rC)
    {
        Room r = new Room((0, 0), rC.Width, rC.Height);
        CreateBaseTileMap(ref r);
        CreateObstacles(ref r, rC);
        CreateDoors(ref r, rC);
        //CreateMonsterSpawns();
        return r;
    }


    void CreateBaseTileMap(ref Room r)
    {
        for (int x = 0; x < r.BoundingBox.Width; x++)
        {
            for (int y = 0; y < r.BoundingBox.Height; y++)
            {
                if (r[x, y] == null)
                {
                    r[x, y] = GenerateTile(x, y, r.BoundingBox, r);
                }
            }
        }
    }

    private void CreateObstacles(ref Room r, RoomConfiguration rC)
    {
        Queue<Seedpoint> seeds = new Queue<Seedpoint>();


        List<Vector3> closed = new List<Vector3>();
        for (int i = 0; i < rC.ObastacleSeedPoints; i++)
        {
            seeds.Enqueue(new Seedpoint(rC.ObstaclePropability, r.BoundingBox.GetPoint()));
        }

        while (seeds.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, seeds.Count);
            Seedpoint seed = seeds.Dequeue();
            closed.Add(seed.Position);

            if (seed.Prob <= 0)
            {
                seeds.Clear();
                continue;
            }

            for (int i = 0; i < XMask.Length; i++)
            {
                Vector3 newPoint = new Vector3(seed.X + XMask[i], seed.Y + YMask[i], 0);
                if (!seeds.Any(x => x.Position == newPoint) && !closed.Contains(newPoint) && r.BoundingBox.IsPointValid(newPoint) && UnityEngine.Random.Range(0f, 1f) < seed.Prob)
                {
                    seeds.Enqueue(Seedpoint.GenerateSeed(seed, newPoint));
                }
            }

            try
            {
                if (r[seed.X, seed.Y].TileType == TileType.edge || UnityEngine.Random.Range(0f, 1f) < 0.7f)
                    continue;
            }
            catch (Exception e)
            {
                continue;
            }



            GameObject obstacle = GameObject.Instantiate(Resources.Load<GameObject>("Room/Test_Obstacle"));
            obstacle.transform.parent = r[seed.X, seed.Y].TileObject.transform;
            obstacle.transform.localPosition = new Vector3(0, 0, 0);
            obstacle.GetComponent<SpriteRenderer>().sortingOrder = 1;

        }
    }


    void CreateDoors(ref Room r, RoomConfiguration rC)
    {
        if (rC.DoorPlacement.HasFlag(DoorPlacement.north))
        {
            Vector3 doorPos = new Vector3(r.BoundingBox.Center.x, r.BoundingBox.MaxY, 0);
            r[(int)doorPos.x, (int)doorPos.y] = new Tile((int)doorPos.x, (int)doorPos.y, r.RoomObject.transform, TileType.door, DoorPlacement.north);

        }
        if (rC.DoorPlacement.HasFlag(DoorPlacement.east))
        {
            Vector3 doorPos = new Vector3(r.BoundingBox.MaxX, r.BoundingBox.Center.y);
            r[(int)doorPos.x, (int)doorPos.y] = new Tile((int)doorPos.x, (int)doorPos.y, r.RoomObject.transform, TileType.door, DoorPlacement.east);
        }
        if (rC.DoorPlacement.HasFlag(DoorPlacement.south))
        {
            Vector3 doorPos = new Vector3(r.BoundingBox.Center.x, r.BoundingBox.MinY, 0);
            r[(int)doorPos.x, (int)doorPos.y] = new Tile((int)doorPos.x, (int)doorPos.y, r.RoomObject.transform, TileType.door, DoorPlacement.south);
        }
        if (rC.DoorPlacement.HasFlag(DoorPlacement.west))
        {
            Vector3 doorPos = new Vector3(r.BoundingBox.MinX, r.BoundingBox.Center.y);
            r[(int)doorPos.x, (int)doorPos.y] = new Tile((int)doorPos.x, (int)doorPos.y, r.RoomObject.transform, TileType.door, DoorPlacement.west);
        }
    }



    Tile GenerateTile(int x, int y, BoundingBox bb, Room r)
    {
        if (x == bb.MinX || x == bb.MaxX || y == bb.MinY || y == bb.MaxY)
        {
            return new Tile(x, y, r.RoomObject.transform, TileType.edge);
        }
        else
            return new Tile(x, y, r.RoomObject.transform);

    }

}

