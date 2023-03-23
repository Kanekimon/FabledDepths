using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class RoomCard
{
    string _id;
    string _name;
    int _difficulty;
    int _doorCount;
    RoomType _type;
    RoomSaveData _room;

    public RoomCard(string id, string name, RoomType type, RoomSaveData room)
    {
        _id = id;
        _name = name;
        _doorCount = (int)DoorPlacementHelper.GetFlagCount(room.Doors);
        _type = type;
        _room = room;
    }

    public string Id { get { return _id; } set { _id = value; } }
    public string Name { get { return _name; } set { _name = value; } }
    public int Difficulty { get { return _difficulty; } set { _difficulty = value; } }
    public RoomType Type { get { return _type; } set { _type = value; } }
    public RoomSaveData Room { get { return _room; } set { _room = value; } }

    public int DoorCount { get { return _doorCount; } set { _doorCount = value; } }
}

