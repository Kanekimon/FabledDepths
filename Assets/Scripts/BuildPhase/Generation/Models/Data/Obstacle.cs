using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.AI;

public enum ObstacleType
{
    grass,
    stone
}

public class Obstacle
{
    private float _x;
    private float _y;
    private ObstacleType _type;


    public float X
    {
        get { return _x; }
        set { _x = value; }
    }
    public float Y
    {
        get { return _y; }
        set { _y = value; }
    }

    public ObstacleType Type
    {
        get { return _type; }
        set { _type = value; }
    }
}
