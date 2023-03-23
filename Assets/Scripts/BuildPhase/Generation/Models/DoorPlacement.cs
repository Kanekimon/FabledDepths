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
    public static UInt32 GetFlagCount(DoorPlacement co)
    {
        UInt32 v = (UInt32)co;
        v = v - ((v >> 1) & 0x55555555);
        v = (v & 0x33333333) + ((v >> 2) & 0x33333333);
        UInt32 count = ((v + (v >> 4) & 0xF0F0F0F) * 0x1010101) >> 24;
        return count;
    }
}
