using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Flags]
public enum DoorPlacement
{
    none = 0,
    north = 1, 
    east = 2,
    south = 4,
    west = 8
}

public static class DoorPlacementHelper
{
    public static int GetFlagCount(DoorPlacement co)
    {
        int count = 0;

        if (co.HasFlag(DoorPlacement.north))
            count++;
        if(co.HasFlag(DoorPlacement.east))
            count++;
        if( co.HasFlag(DoorPlacement.west))
            count++;
        if (co.HasFlag(DoorPlacement.south))
            count++;

        return count;
    }
}
