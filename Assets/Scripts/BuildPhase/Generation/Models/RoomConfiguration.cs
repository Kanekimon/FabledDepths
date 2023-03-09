using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;



[Serializable]
public class RoomConfiguration
{
    public RoomType RoomType;
    public float ObstaclePropability;
    public int ObastacleSeedPoints;
    public DoorPlacement DoorPlacement;

    public RoomConfiguration(RoomConfiguration config)
    {
        RoomType = config.RoomType;
        ObstaclePropability = config.ObstaclePropability;
        ObastacleSeedPoints = config.ObastacleSeedPoints;
    }

    public RoomConfiguration() { }
}


