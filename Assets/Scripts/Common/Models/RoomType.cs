using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


[Flags]
public enum RoomType
{
    start = 1,
    monster = 2,
    treasure = 4,
    boss = 8,
    trap = 16, 
    puzzle = 32,
    shop = 64
}


