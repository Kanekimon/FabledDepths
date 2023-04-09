using Assets.Scripts.Room_Editor.Manager;
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


    static Vector2[] circleOffsets;
    static int[] radiusToMaxIndex;

    List<ClickHandler> highlightedTiles = new List<ClickHandler>();


    public (bool Active, int MouseButton) IsDragging;

    bool mouseState;
    public int BrushSize = 1;


    public int ActiveTileIndex = 0;

    public ClickHandler CurrentlyOver;

    public Sprite GetTileSprite(int index)
    {
        return TileSprites[index];
    }

    public Sprite GetActiveSprite()
    {
        return RoomEditorUIMananger.Instance.ActiveTile.PlaceablePrefab.GetComponent<SpriteRenderer>().sprite;
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

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if(Input.mouseScrollDelta.magnitude > 0)
            {
                Debug.Log(Input.mouseScrollDelta.y);    
                BrushSize = Mathf.Clamp(BrushSize + (int)Input.mouseScrollDelta.y,1, 10) ;
                ClickHandler clk = CurrentlyOver;
                ResetBrushPreview();
                PreviewBrush(CurrentlyOver);
            }
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


    public void PreviewBrush(ClickHandler over)
    {
        CurrentlyOver = over;
        InitCircle(RoomEditorBuildManager.Instance.BrushSize);

        foreach (Vector2 point in circleOffsets)
        {
            ClickHandler ch = RoomEditorManager.Instance.GetTileAtIndex((int)(over.X + point.x), (int)(over.Y + point.y));
            if (ch == null)
                continue;

            highlightedTiles.Add(ch);

            ch.Highlight();
        }
    }

    public void ResetBrushPreview()
    {
        foreach (var tile in highlightedTiles)
        {
            tile.ResetColor();
        }
    }


    static void InitCircle(int radius)
    {
        List<Vector2> results = new List<Vector2>((radius * 2) * (radius * 2));

        for (int y = -radius; y <= radius; y++)
            for (int x = -radius; x <= radius; x++)
                results.Add(new Vector2(x, y));

        circleOffsets = results.OrderBy(p =>
        {
            int dx = (int)p.x;
            int dy = (int)p.y;
            return dx * dx + dy * dy;
        })
        .TakeWhile(p =>
        {
            int dx = (int)p.x;
            int dy = (int)p.y;
            var r = dx * dx + dy * dy;
            return r < radius * radius;
        })
        .ToArray();

        radiusToMaxIndex = new int[radius];
        for (int r = 0; r < radius; r++)
            radiusToMaxIndex[r] = FindLastIndexWithinDistance(circleOffsets, r);
    }

    static int FindLastIndexWithinDistance(Vector2[] offsets, int maxR)
    {
        int lastIndex = 0;

        for (int i = 0; i < offsets.Length; i++)
        {
            var p = offsets[i];
            int dx = (int)p.x;
            int dy = (int)p.y;
            int r = dx * dx + dy * dy;

            if (r > maxR * maxR)
            {
                return lastIndex + 1;
            }
            lastIndex = i;
        }

        return 0;
    }

}

