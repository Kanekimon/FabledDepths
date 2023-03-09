using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class PlaceholderAddRoom : BaseRoom
{

    Sprite original;
    Sprite current;

    private void Awake()
    {
        current = original = GetComponent<SpriteRenderer>().sprite;
        RoomObject = this.gameObject;
    }


    private void OnMouseDown()
    {
        BuildManager.Instance.RegisterRoom(this);
    }

}

