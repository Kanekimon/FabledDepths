﻿using Assets.Scripts.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class RoomEditorManager : Singleton<RoomEditorManager>
{
    public GameObject[,] RoomMap = new GameObject[40, 20];

    [NonSerialized]
    public List<Placeable> Placeables = new List<Placeable>();

    public List<DoorPlacement> Doors = new List<DoorPlacement>();

    protected virtual void Awake()
    {
        base.Awake();
        LoadPlaceables();
    }

    private void Start()
    {
        this.transform.position -= new Vector3((RoomMap.GetLength(0) / 2) - 1, (RoomMap.GetLength(1) / 2) - 1);
        int width =  RoomMap.GetLength(0);
        int height = RoomMap.GetLength(1);


        for (int x = 0; x < RoomMap.GetLength(0); x++)
        {
            for (int y = 0; y < RoomMap.GetLength(1); y++)
            {
                GameObject g = null;
                if (x == 0 || y == 0 || x == RoomMap.GetLength(0) - 1 || y == RoomMap.GetLength(1) - 1)
                {
                    if (((x == (width / 2)-1 || x == (width / 2)) && (y == height-1 || y == 0)) || 
                        ((x == width-1 || x == 0) && (y == (height/2)-1 || y == (height/2))))
                    {
                        g = Instantiate(Resources.Load<GameObject>("Prefabs/RoomEditor/Tile_Door"));
                        g.GetComponent<DoorHandler>().Placement = GetDoorPlacementFromCoords.GetPlacement(RoomMap, x, y);
                    }
                    else
                    {
                        g = Instantiate(Resources.Load<GameObject>("Prefabs/RoomEditor/Tile_Blocked"));
                    }


                }
                else
                {
                    g = Instantiate(Resources.Load<GameObject>("Prefabs/RoomEditor/Tile_Placeholder"));
                    g.GetComponent<ClickHandler>().SetIndex(x, y);
                }

                g.transform.parent = transform;
                g.transform.localPosition = new Vector2((float)x, (float)y);

                RoomMap[x, y] = g;
            }
        }
    }

    public ClickHandler GetTileAtIndex(int x, int y)
    {
        if (x <= 0 || y <= 0 || x >= RoomMap.GetLength(0) - 1 || y >= RoomMap.GetLength(1) - 1)
            return null;

        return RoomMap[x, y].GetComponent<ClickHandler>();
    }


    public void LoadPlaceables()
    {
        List<Placeable> placeables = Resources.LoadAll<Placeable>("ScriptableObject/RoomEditor").ToList();
        if(placeables != null && placeables.Count > 0)
        {
            Placeables = placeables;
        }
    }

    void SavePlaceable()
    {

    }

    internal void ToggleDoor(DoorPlacement placement, bool v)
    {
        if (v && !Doors.Contains(placement))
        {
            Doors.Add(placement);
            GameObject.FindGameObjectsWithTag("door").Where(x => x.GetComponent<DoorHandler>().Placement == placement).ToList().ForEach(x =>
            {
                x.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Room/Textures/Test_Door");
            });

        }

        if (!v && Doors.Contains(placement))
        {
            Doors.Remove(placement);
            GameObject.FindGameObjectsWithTag("door").Where(x => x.GetComponent<DoorHandler>().Placement == placement).ToList().ForEach(x =>
            {
                x.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Room/Textures/Add_Door");
            });
        }
    }
}

