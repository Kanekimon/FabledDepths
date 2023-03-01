using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CellularAutomata : IGenerator
{
    public Room GenerateRoom(RoomConfiguration rC)
    {
        Room r = new Room((0,0));
        
        for(int x = 0; x < rC.BoundingBox.Width; x++)
        {
            for(int y = 0; y < rC.BoundingBox.Height; y++)
            {
                if (r[x,y] == null)
                {
                    r[x, y] = new Tile(x, y, r.RoomObject.transform);
                }
            }
        }

        return r;

    }
}

