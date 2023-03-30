using NUnit.Framework.Constraints;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Color = UnityEngine.Color;

public class ClickHandler : MonoBehaviour
{
    static Vector2[] circleOffsets;
    static int[] radiusToMaxIndex;
    Color c;
    Sprite org;
    bool MouseDown;
    int ActiveMouseButton;
    SpriteRenderer sRenderer;
    private int x;
    private int y;


    private void Start()
    {
        sRenderer = GetComponent<SpriteRenderer>();
        c = sRenderer.material.color;
        org = sRenderer.sprite;
    }

    public void SetIndex(int x, int y)
    {
        this.x = x;
        this.y = y;
    }


    //private void OnMouseDrag()
    //{
    //    RoomEditorBuildManager.Instance.IsDragging = true;
    //}

    //private void OnMouseUp()
    //{
    //    RoomEditorBuildManager.Instance.IsDragging = false;
    //}



    private void OnMouseOver()
    {
        sRenderer.material.color = Color.green;

        if (Input.GetMouseButtonDown(0))
            PlaceTile();

        if (Input.GetMouseButtonDown(1))
            PlaceTile(true);

        if(Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1))
        {
            MouseDown = false;
            ActiveMouseButton = -1;
            RoomEditorBuildManager.Instance.IsDragging = (MouseDown, ActiveMouseButton);
        }


        if (RoomEditorBuildManager.Instance.IsDragging.Active)
        {
            PlaceTile(RoomEditorBuildManager.Instance.IsDragging.MouseButton == 0 ? false : true);
        }
    }

    private void OnMouseExit()
    {
        this.GetComponent<Renderer>().material.color = c;
    }

    void PlaceTile(bool unset = false)
    {
        MouseDown = true;
        ActiveMouseButton = unset ? 1 : 0;

        RoomEditorBuildManager.Instance.IsDragging = (MouseDown, ActiveMouseButton);
        InitCircle(RoomEditorBuildManager.Instance.BrushSize);

        foreach(Vector2 point in circleOffsets)
        {
            ClickHandler ch = RoomEditorManager.Instance.GetTileAtIndex((int)(this.x + point.x), (int)(this.y + point.y));
            if (ch == null)
                continue;

            if (!unset)
                ch.GetComponent<SpriteRenderer>().sprite = RoomEditorBuildManager.Instance.GetActiveSprite();
            else
                ch.GetComponent<SpriteRenderer>().sprite = org;
        }


        //if (!unset)
        //    sRenderer.sprite = RoomEditorBuildManager.Instance.GetTileSprite(0);
        //else
        //    sRenderer.sprite = org;

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

