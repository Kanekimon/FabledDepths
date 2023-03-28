using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RoomSaveData
{
    public int XIndex;
    public int YIndex;

    public int RoomXCoord;
    public int RoomYCoord;

    public int RoomWidth;
    public int RoomHeight;

    public string TileString;
    public string ObstacleString;

    public DoorPlacement Doors;


    public static RoomSaveData SaveRoom(Room r)
    {
        RoomSaveData rsd = new RoomSaveData();

        rsd.RoomXCoord = r.BoundingBox.MinX;
        rsd.RoomYCoord = r.BoundingBox.MinY;

        rsd.RoomWidth = r.BoundingBox.Width;
        rsd.RoomHeight = r.BoundingBox.Height;

        rsd.XIndex = r.Index.X;
        rsd.YIndex = r.Index.Y;

        rsd.Doors = r.Doors;

        foreach (Tile t in r.Tiles)
        {
            if (t.Placement != DoorPlacement.none)
                rsd.TileString += $"[X:{(int)t.X},Y:{t.Y},T:{t.TileType},D:{t.Placement}];";
            else
                rsd.TileString += $"[X:{(int)t.X},Y:{t.Y},T:{t.TileType}];";

        }

        foreach (Obstacle o in r.Obstacles)
        {
            rsd.ObstacleString += $"[X:{(int)o.X},Y:{o.Y},T:{o.Type}];";
        }


        return rsd;
    }

    public static Room LoadRoom(RoomSaveData rsd, bool generateObject = true)
    {
        Room r = new Room();

        r.Init((rsd.XIndex, rsd.YIndex), rsd.RoomWidth, rsd.RoomHeight, generateObject);
        r.Doors = rsd.Doors;

        List<string> tileString = rsd.TileString.Split(";").ToList();

        foreach (string tile in tileString)
        {
            if (string.IsNullOrEmpty(tile))
                continue;
            int tileX = -1;
            int tileY = -1;
            TileType tt = TileType.normal;
            DoorPlacement dp = DoorPlacement.none;

            List<string> pars = tile.Split(",").ToList();
            foreach (string par in pars)
            {
                string partemp = par.Replace("[", "").Replace("]", "");
                if (partemp.Contains("X:"))
                {
                    tileX = int.Parse(partemp.Replace("X:", ""));
                }
                else if (partemp.Contains("Y:"))
                {
                    tileY = int.Parse(partemp.Replace("Y:", ""));
                }
                else if (partemp.Contains("D:"))
                {
                    dp = (DoorPlacement)Enum.Parse(typeof(DoorPlacement), partemp.Replace("D:", ""));
                }
                else if (partemp.Contains("T:"))
                {
                    try
                    {
                        tt = (TileType)Enum.Parse(typeof(TileType), partemp.Replace("T:", ""));
                    }
                    catch (Exception e)
                    {
                        Debug.Log($"TileType string: {partemp}");
                    }
                }
            }


            r[tileX, tileY] = new Tile(tileX, tileY, tt, dp, generateObject ? r.RoomObject.transform : null);
        }

        foreach (string obstacle in rsd.ObstacleString.Split(";").ToList())
        {
            if (string.IsNullOrEmpty(obstacle))
                continue;
            int tileX = -1;
            int tileY = -1;
            ObstacleType ot = ObstacleType.grass;

            List<string> pars = obstacle.Split(",").ToList();
            foreach (string par in pars)
            {
                string partemp = par.Replace("[", "").Replace("]", "");
                if (partemp.Contains("X:"))
                {
                    tileX = int.Parse(partemp.Replace("X:", ""));
                }
                else if (partemp.Contains("Y:"))
                {
                    tileY = int.Parse(partemp.Replace("Y:", ""));
                }
                else if (partemp.Contains("T:"))
                {
                    try
                    {
                        ot = (ObstacleType)Enum.Parse(typeof(ObstacleType), partemp.Replace("T:", ""));
                    }
                    catch (Exception e)
                    {
                        Debug.Log($"TileType string: {partemp}");
                    }
                }
            }


            if (r[tileX, tileY].TileObject != null)
            {
                GameObject obst = GameObject.Instantiate(Resources.Load<GameObject>($"Room/Obstacles/{ot}"));
                obst.transform.parent = r[tileX, tileY].TileObject.transform;
                obst.transform.localPosition = Vector2.zero;
            }
        }

        return r;
    }



    public string ToJson()
    {
        JsonSerializerSettings settings = new JsonSerializerSettings()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };
        return JsonConvert.SerializeObject(this, settings);
    }


}

