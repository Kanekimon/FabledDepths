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

