using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Node
{
    public Vector2Int Index;
    public int G = 0;
    public int H = 0;
    public float F { get { return G + H; } }


    public Node Previous = null;
}

public class AStar
{
    public static List<Vector2Int> GeneratePath(Room r, Vector2Int start, Vector2Int end)
    {

        bool[,] grid = new bool[r.BoundingBox.Width, r.BoundingBox.Height];

        List<Node> open = new List<Node>();
        List<Node> closed = new List<Node>();

        open.Add(CreateNode(start));

        Node endNode = CreateNode(end);


        while (open.Any())
        {
            Node currentNode = GetNodeWithSmallestCost(open);
            GetAdjacend(r, currentNode, endNode, ref open, closed);

            if (currentNode.Index == endNode.Index)
            {
                return BackTracePath(currentNode);
            }

            open.Remove(currentNode);
            closed.Add(currentNode);
        }

        return new List<Vector2Int>();

    }

    static Node GetNodeWithSmallestCost(List<Node> open)
    {
        return open.OrderBy(x => x.F).FirstOrDefault();
    }

    static List<Vector2Int> BackTracePath(Node endNode)
    {
        List<Vector2Int> path = new List<Vector2Int>();

        Node current = endNode.Previous;
        while (current.Previous != null)
        {
            path.Add(current.Index);
            current = current.Previous;
        }
        path.Reverse();
        return path;
    }

    static void GetAdjacend(Room r, Node current, Node end, ref List<Node> open, List<Node> closed)
    {

        List<Node> validAdjacend = new List<Node>();

        for (int i = 0; i < IterationMasks.FourX.Length; i++)
        {
            int xN = current.Index.x + IterationMasks.FourX[i];
            int yN = current.Index.y + IterationMasks.FourY[i];

            if ((xN < 0 || xN >= r.BoundingBox.Width - 1 || yN < 0 || yN >= r.BoundingBox.Height - 1
                || xN == 0 || yN == 0) && !(end.Index.x == xN && end.Index.y == yN)) continue;

            Vector2Int index = new Vector2Int(xN, yN);

            if (closed.Any(x => x.Index == index))
                continue;

            Node ad = CreateNode(index, current, end);


            if (open.Any(x => x.Index == ad.Index))
            {
                Node existing = open.Where(x => x.Index == ad.Index).FirstOrDefault();
                if (ad.F < existing.F)
                {
                    existing.G = ad.G;
                    existing.H = ad.H;
                    existing.Previous = ad.Previous;
                }
            }
            else
            {
                open.Add(ad);
            }

        }
    }

    static Node CreateNode(Vector2Int index, Node previous, Node end)
    {
        return new Node()
        {
            Index = index,
            Previous = previous,
            G = previous.G + 1,
            H = Convert.ToInt32(Vector2Int.Distance(index, end.Index))
        };
    }

    static Node CreateNode(Vector2Int index)
    {
        Node n = new Node();
        n.Index = index;
        n.H = int.MaxValue;
        return n;
    }

}
