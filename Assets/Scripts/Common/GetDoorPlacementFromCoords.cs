using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Common
{
    public static class GetDoorPlacementFromCoords
    {

        public static DoorPlacement GetPlacement(GameObject[,] map, int x, int y) 
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            if ((x == width / 2 || x == (width / 2 - 1)) && (y == height - 1))
                return DoorPlacement.north;
            else if (x == width - 1 && (y == height / 2 || y == (height / 2 - 1)))
                return DoorPlacement.east;
            else if ((x == width/2 || x == (width/2)-1) && y == 0)
                return DoorPlacement.south;
            else if (x == 0 && (y == height / 2 || y == (height / 2 - 1)))
                return DoorPlacement.west;


            return DoorPlacement.none;
        }

    }
}
