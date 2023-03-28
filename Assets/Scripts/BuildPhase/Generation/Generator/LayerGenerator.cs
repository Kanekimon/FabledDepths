using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class LayerGenerator : IGenerator
{
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
        CreatePaths(ref r, rC, parent);
        CreateObstacles(ref r, rC);
    }


    void CreateBaseTileMap(ref Room r, RoomConfiguration rC, Transform parent = null)
    {
        for (int x = 0; x < r.BoundingBox.Width; x++)
        {
            for (int y = 0; y < r.BoundingBox.Height; y++)
            {
                if (r[x, y] == null)
                {
                    r[x, y] = GenerateTile(x, y, r.BoundingBox, r, TileType.normal, parent);
                }
            }
        }
    }


    void CreatePaths(ref Room r, RoomConfiguration rC, Transform parent = null)
    {
        




        int doorCount = (int)DoorPlacementHelper.GetFlagCount(r.Doors);
        if(doorCount == 1)
        {
            Debug.Log("Test");
        }

        try
        {
            List<DoorPlacement> doors = AddDoorsToList(ref r);

            if (doors.Count <= 1)
                return;

            DoorPlacement start = doors.ElementAt(Random.Range(0, doors.Count));
            doors.Remove(start);
            DoorPlacement end = doors.ElementAt(Random.Range(0, doors.Count));
            doors.Remove(end);

            List<Vector2Int> path = new List<Vector2Int>();

            Vector2Int startPoint = DoorPosition(r, start);

            if (BuildManager.Instance.Settings.Subpaths > 1)
            {
                int randomSubPathCount = Random.Range(2, BuildManager.Instance.Settings.Subpaths + 1);

                List<Vector2Int> points = new List<Vector2Int>();
                int possbilePathCount = randomSubPathCount - 2;

                for(int i = 0;i < randomSubPathCount; i++)
                {
                    points.Add(new Vector2Int(Random.Range(1, r.BoundingBox.Width - 1), Random.Range(1, r.BoundingBox.Height - 1)));
                }

                foreach (Vector2Int point in points)
                {
                    path.AddRange(AStar.GeneratePath(r, startPoint, point));
                    path.Add(point);
                    startPoint = point;
                }
            }

            path.AddRange(AStar.GeneratePath(r, startPoint, DoorPosition(r, end)));

            foreach (Vector2Int index in path)
            {
                r[index.x, index.y] = new Tile(index.x, index.y, TileType.path, DoorPlacement.none, parent);
            }


            //List<Vector2Int> path = AStar.GeneratePath(r, DoorPosition(r, start), DoorPosition(r, end));


            if (doors.Count > 0)
            {
                for(int i = doors.Count-1; i >= 0; i--)
                {
                    Vector2Int randomPathPoint = path.ElementAt(Random.Range(0, path.Count));

                    

                    foreach (Vector2Int index in AStar.GeneratePath(r, DoorPosition(r, doors[i]), randomPathPoint))
                    {
                        r[index.x, index.y] = new Tile(index.x, index.y, TileType.path, DoorPlacement.none, parent);
                    }

                    doors.RemoveAt(i);
                }
            }






            //while (!AllCanBeReached(connectionMatrix))
            //{
            //    int notYetReached = canBeReached.Count(x => !x.Value);

            //    KeyValuePair<DoorPlacement, List<DoorPlacement>> to = connectionMatrix.Where(x => x.Value.Count == 0).ElementAt(Random.Range(0, notYetReached));

            //    KeyValuePair<DoorPlacement, List<DoorPlacement>> from = connectionMatrix.Where(x => x.Key != to.Key).ElementAt(Random.Range(0, connectionMatrix.Count - 1));

            //    foreach (Vector2Int index in AStar.GeneratePath(r, DoorPosition(r, to.Key), DoorPosition(r, from.Key)))
            //    {
            //        r[index.x, index.y] = new Tile(index.x, index.y, TileType.path, DoorPlacement.none, parent);
            //    }

            //    connectionMatrix[to.Key].Add(from.Key);
            //    connectionMatrix[from.Key].Add(to.Key);
            //}
        }
        catch (Exception ex)
        {
            Debug.Log("Door count: " + doorCount);
        }

    }


    bool AllCanBeReached(Dictionary<DoorPlacement, List<DoorPlacement>> matrix)
    {
        if (matrix.Any(x => x.Key != DoorPlacement.none && x.Value.Count == 0))
            return false;

        bool allReached = true;
        foreach(KeyValuePair<DoorPlacement, List<DoorPlacement>> door in matrix)
        {
            foreach(KeyValuePair<DoorPlacement, List<DoorPlacement>> otherdoor in matrix.Where(x => x.Key != door.Key))
            {
                if (otherdoor.Value.Count == 0)
                    return false;

                allReached &= CanReachDoor(matrix, otherdoor, door.Key);
                
            }

           // allReached &= matrix.Any(x => x.Value == door.Key);
        }

        return true;
    }

    bool CanReachDoor(Dictionary<DoorPlacement, List<DoorPlacement>> matrix, KeyValuePair<DoorPlacement, List<DoorPlacement>> otherDoor, DoorPlacement root)
    {
        bool canBeReached = matrix[root].Contains(otherDoor.Key);

        if (canBeReached)
            return true;

        foreach(DoorPlacement connectedTo in otherDoor.Value)
        {
            if(connectedTo == root) return true;

            canBeReached |= CanReachDoor(matrix, matrix.Where(x => x.Key == connectedTo).FirstOrDefault(), root);
        }
        return canBeReached;
    }


    Vector2Int DoorPosition(Room r, DoorPlacement placement)
    {
        switch (placement)
        {
            case DoorPlacement.none:
                return new Vector2Int(-1, -1);
                break;
            case DoorPlacement.north:
                return new Vector2Int((int)r.BoundingBox.LocalCenter.XminYmin.x, (int)r.BoundingBox.Height - 1);
                break;
            case DoorPlacement.east:
                return new Vector2Int((int)r.BoundingBox.Width - 1, (int)r.BoundingBox.LocalCenter.XminYmin.y);
                break;
            case DoorPlacement.south:
                return new Vector2Int((int)r.BoundingBox.LocalCenter.XminYmin.x, 0);
                break;
            case DoorPlacement.west:
                return new Vector2Int(0, (int)r.BoundingBox.LocalCenter.XminYmin.y);
                break;
            default:
                return new Vector2Int(-1, -1);
        }
    }

    Dictionary<DoorPlacement, List<DoorPlacement>> CreateConnectionMatrix(ref Room r)
    {
        if ((int)DoorPlacementHelper.GetFlagCount(r.Doors) == 0)
        {
            return new Dictionary<DoorPlacement, List<DoorPlacement>>();
        }

        Dictionary<DoorPlacement, List<DoorPlacement>> canBeReached = new();

        if (r.Doors.HasFlag(DoorPlacement.north))
        {
            canBeReached.Add(DoorPlacement.north, new List<DoorPlacement>());
        }

        if (r.Doors.HasFlag(DoorPlacement.east))
        {
            canBeReached.Add(DoorPlacement.east, new List<DoorPlacement>());
        }
        if (r.Doors.HasFlag(DoorPlacement.south))
        {
            canBeReached.Add(DoorPlacement.south, new List<DoorPlacement>());
        }
        if (r.Doors.HasFlag(DoorPlacement.west))
        {
            canBeReached.Add(DoorPlacement.west, new List<DoorPlacement>());
        }

        return canBeReached;
    }



        List<DoorPlacement> AddDoorsToList(ref Room r)
    {
        if ((int)DoorPlacementHelper.GetFlagCount(r.Doors) == 0)
        {
            return new List<DoorPlacement>();
        }

        List<DoorPlacement> canBeReached = new();

        if (r.Doors.HasFlag(DoorPlacement.north))
        {
            canBeReached.Add(DoorPlacement.north);
        }

        if (r.Doors.HasFlag(DoorPlacement.east))
        {
            canBeReached.Add(DoorPlacement.east);
        }
        if (r.Doors.HasFlag(DoorPlacement.south))
        {
            canBeReached.Add(DoorPlacement.south);
        }
        if (r.Doors.HasFlag(DoorPlacement.west))
        {
            canBeReached.Add(DoorPlacement.west);
        }

        return canBeReached;
    }


    private void CreateObstacles(ref Room r, RoomConfiguration rC)
    {
        int x = r.BoundingBox.Width - 1;
        int y = r.BoundingBox.Height - 1;

        for(int rand = 0; rand < Mathf.FloorToInt(r.Tiles.Count * 0.65f); rand++)
        {

            (int i, int j) = GetRandomObstaclePoint(r, x, y);

            int obstaclesAdjacend = 0;
            for (int t = 0; t < IterationMasks.EightX.Length; t++)
            {
                if (r.Obstacles.Any(x => x.X == i + IterationMasks.EightX[t] && x.Y == j + IterationMasks.EightY[t]))
                    obstaclesAdjacend++;
            }

            if (obstaclesAdjacend <= 4)
            {
                int maxValue = Enum.GetValues(typeof(ObstacleType))
               .Cast<int>()
               .Max();

                int randomIndex = UnityEngine.Random.Range(0, maxValue + 1);
                while (r[i, j].TileType == TileType.path && (randomIndex % 2 != 0))
                {
                    randomIndex = UnityEngine.Random.Range(0, maxValue + 1);
                }

                Obstacle obst = new Obstacle() { X = i, Y = j, Type = (ObstacleType)randomIndex };
                r.Obstacles.Add(obst);

                if (r[i, j].TileObject != null)
                {
                    GameObject inst = GameObject.Instantiate(Resources.Load<GameObject>($"Room/Obstacles/{obst.Type}"));
                    inst.transform.parent = r[i, j].TileObject.transform;
                    inst.transform.localPosition = Vector3.zero;
                }
            }
        }

    }

    private (int x, int y) GetRandomObstaclePoint(Room r, int maxX, int maxY)
    {
        
        int i = UnityEngine.Random.Range(1, maxX);
        int j = UnityEngine.Random.Range(1, maxY);

        while(r.Obstacles.Any(x => x.X == i && x.Y == j))
        {
            i = UnityEngine.Random.Range(1, maxX);
            j = UnityEngine.Random.Range(1, maxY);
        }

        return (i, j);

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


    Tile GenerateTile(int x, int y, BoundingBox bb, Room r, TileType tileType = TileType.normal, Transform parent = null)
    {
        int xOffset = r.Index.X * bb.Width;
        int yOffset = r.Index.Y * bb.Height;


        if (x == bb.MinX - xOffset || x == bb.MaxX - xOffset || y == bb.MinY - yOffset || y == bb.MaxY - yOffset)
        {
            return new Tile(x, y, TileType.edge, DoorPlacement.none, parent);
        }
        else
            return new Tile(x, y, tileType, DoorPlacement.none, parent);

    }

}

