using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    private IGenerator _generator;
    private Room r;
    public RoomConfiguration Configuration;

    public RoomGenerator()
    {
        _generator = new LayerGenerator();

    }

    public void GenerateRoom()
    {
        List<GameObject> rooms = GameObject.FindGameObjectsWithTag("Room").ToList();
        for (int i = rooms.Count - 1; i >= 0; i--)
        {
            DestroyImmediate(rooms[i]);
        }
        r = _generator.GenerateRoom(Configuration);

        RoomGenerationManager.Instance.SaveRoomAsJson(r);

    }



}

