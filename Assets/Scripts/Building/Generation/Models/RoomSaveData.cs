using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RoomSaveData
{
    public int RoomXCoord;
    public int RoomYCoord;

    public int RoomWidth;
    public int RoomHeight;

    public string TileString;

    public string ObstacleString;




    public static RoomSaveData SaveRoom(Room r)
    {
        RoomSaveData rsd = new RoomSaveData();

        rsd.RoomXCoord = r.BoundingBox.MinX;
        rsd.RoomYCoord = r.BoundingBox.MinY;

        rsd.RoomWidth = r.BoundingBox.Width;
        rsd.RoomHeight = r.BoundingBox.Height;


        foreach(Tile t in r.Tiles)
        {
            if(t.Placement != DoorPlacement.none)
                rsd.TileString += $"[X:{(int)t.X},Y:{t.Y},T:{t.TileType},D:{t.Placement}];";
            else
                rsd.TileString += $"[X:{(int)t.X},Y:{t.Y},T:{t.TileType}];";

        }
        return rsd;
    }

    public static Room LoadRoom(RoomSaveData rsd)
    {
        Room r = new Room((rsd.RoomXCoord, rsd.RoomYCoord), rsd.RoomWidth, rsd.RoomHeight);


        List<string> tileString = rsd.TileString.Split(";").ToList();

        foreach(string tile in tileString)
        {
            if (string.IsNullOrEmpty(tile))
                continue;
            int tileX = -1;
            int tileY = -1;
            TileType tt = TileType.normal;
            DoorPlacement dp = DoorPlacement.none;

            List<string> pars = tile.Split(",").ToList();
            foreach(string par in pars)
            {
                string partemp = par.Replace("[", "").Replace("]", "");
                if (partemp.Contains("X:"))
                {
                    tileX = int.Parse(partemp.Replace("X:",""));
                }
                else if (partemp.Contains("Y:"))
                {
                    tileY = int.Parse(partemp.Replace("Y:", ""));
                }
                else if (partemp.Contains("D:"))
                {
                    dp = (DoorPlacement)Enum.Parse(typeof(DoorPlacement), partemp.Replace("D:", ""));
                }
                else if(partemp.Contains("T:"))
                {
                    try
                    {
                        tt = (TileType)Enum.Parse(typeof(TileType), partemp.Replace("T:", ""));
                    }catch(Exception e)
                    {
                        Debug.Log($"TileType string: {partemp}");
                    }
                }
            }
            r[tileX, tileY] = new Tile(tileX, tileY, r.RoomObject.transform, tt, dp);
        }
        return r;

    }


}

