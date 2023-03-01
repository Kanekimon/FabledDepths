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

    public Room GenerateRoom(RoomConfiguration rC)
    {
        Room r = new Room((0, 0));
        CreateBaseTileMap(ref r, rC);
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

