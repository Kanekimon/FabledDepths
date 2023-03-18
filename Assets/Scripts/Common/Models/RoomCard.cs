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
    RoomType _type;
    RoomSaveData _room;


    public string Id { get { return _id; } set { _id = value; } }
    public string Name { get { return _name; } set { _name = value; } }
    public int Difficulty { get { return _difficulty; } set { _difficulty = value; } }
    public RoomType Type { get { return _type; } set { _type = value; } }
    public RoomSaveData Room { get { return _room; } set { _room = value; } }

}

