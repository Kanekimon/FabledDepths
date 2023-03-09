using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AdjacendRoom
{
    public (int X, int Y) Index { get; set; }



    public BaseRoom room { get; set; }
}



public abstract class BaseRoom : MonoBehaviour
{
    private (int X, int Y) _index;

    public (int X, int Y) Index
    {
        get
        {
            return _index;
        } 
        set
        {
            _index = value;
        }
    }
    public List<AdjacendRoom> Adjacend = new List<AdjacendRoom>();
    [JsonIgnore] private GameObject _roomObject;

    [JsonIgnore]
    public GameObject RoomObject
    {
        get { return _roomObject; }
        internal set { _roomObject = value; }
    }

    public void CopyRoomRefs(BaseRoom newRoom)
    {
        foreach(AdjacendRoom a in Adjacend)
        {
            if (!newRoom.Adjacend.Contains(a))
            {
                newRoom.Adjacend.Add(a);
                a.room.Adjacend.Where(x => x.Index == this.Index).FirstOrDefault().room = newRoom;
            }
        }
    }

 


}