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

    protected virtual void Awake()
    {
        base.Awake();
        LoadPlaceables();
    }

    private void Start()
    {
        this.transform.position -= new Vector3((RoomMap.GetLength(0) / 2) - 1, (RoomMap.GetLength(1) / 2) - 1);

        for (int x = 0; x < RoomMap.GetLength(0); x++)
        {
            for (int y = 0; y < RoomMap.GetLength(1); y++)
            {
                GameObject g = null;
                if (x == 0 || y == 0 || x == RoomMap.GetLength(0) - 1 || y == RoomMap.GetLength(1) - 1)
                {
                    g = Instantiate(Resources.Load<GameObject>("Prefabs/RoomEditor/Tile_Blocked"));
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
        if (x < 0 || y < 0 || x >= RoomMap.GetLength(0) - 1 || y >= RoomMap.GetLength(1) - 1)
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
}

