using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RoomEditorBuildManager : Singleton<RoomEditorBuildManager>
{
    [SerializeField]
    public List<Sprite> TileSprites;

    public (bool Active, int MouseButton) IsDragging;

    bool mouseState;
    public int BrushSize;


    public int ActiveTileIndex = 0;

    public Sprite GetTileSprite(int index)
    {
        return TileSprites[index];
    }

    public Sprite GetActiveSprite()
    {
        return TileSprites[ActiveTileIndex];
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            mouseState = true;
        if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
            mouseState = false;

        if (IsDragging.Active && !mouseState)
        {
            IsDragging.Active = mouseState;
        }


        if(Input.mouseScrollDelta.magnitude > 0)
        {
            if(Input.mouseScrollDelta.y > 0)
            {
                ActiveTileIndex = Mathf.Clamp(ActiveTileIndex +1, 0, TileSprites.Count-1);
            }
            else if(Input.mouseScrollDelta.y < 0)
            {
                ActiveTileIndex = Mathf.Clamp(ActiveTileIndex - 1, 0, TileSprites.Count-1);

            }
        }
    }

}

