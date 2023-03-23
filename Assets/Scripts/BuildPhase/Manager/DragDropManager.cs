using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class DragDropManager : Singleton<DragDropManager>
{
    public Draggable CurrentlyDragging;
    public bool IsDragging;
    public Dropable CurrentlyOver;
    public Vector3 originialPos;
    public Camera Cam;

    private void Update()
    {

    }

    public void RegisterDraggable(Draggable dragObject)
    {
        CurrentlyDragging = dragObject;
        originialPos = dragObject.transform.position;
        //CurrentlyDragging.gameObject.GetComponent<BoxCollider2D>().enabled = false;
    }

    public void UnregisterDraggable(Draggable dragObject)
    {
        if(CurrentlyOver != null)
        {
            BuildManager.Instance.RegisterRoom(CurrentlyOver.GetComponent<PlaceholderRoom>(), CurrentlyDragging.GetComponent<Room>());
        }
        else
        {
            if (dragObject.gameObject.tag.Equals("card"))
            {
                Destroy(dragObject.gameObject);
            }
            CurrentlyDragging.transform.position = originialPos;
        }

        if(CurrentlyDragging != null)
            CurrentlyDragging.GetComponent<BoxCollider2D>().enabled = true; 
        CurrentlyDragging = null;
    }


    public void RegisterDropable(Dropable d)
    {
        CurrentlyOver = d;
    }

    public void UnregisterDropable(Dropable d)
    {
        CurrentlyOver = null;
        d.gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

    public void CreateDragInstance()
    {
        GameObject card = Instantiate(Resources.Load<GameObject>("Room/Card"));
        card.GetComponent<Draggable>().ForceToDrag();
    }

    public bool IsOverDropArea()
    {
        if(CurrentlyOver != null)
            return true;

        return false;
    }

    public bool CanDropOnPlaceholder(GameObject placeholder, string card_id)
    {
        RoomCard rc = Dev_Card_Builder.Instance.GetCardFromDeck(card_id);
        KeyValuePair<(int X, int Y), BaseRoom> pair = BuildManager.Instance.RoomMap.Where(x => x.Value.RoomObject == placeholder).FirstOrDefault();

        bool isValid = true;

        //----------North -> Room in North direction needs a door in south direction -> if no north door, door in north directions musnt have a south door-----------------------------------------------------------------------------------
        if (rc.Room.Doors.HasFlag(DoorPlacement.north))
        {
            BaseRoom north = BuildManager.Instance.GetRoom(pair.Key.X, pair.Key.Y + 1).Value;
            if(north != null && north is Room)
            {
               isValid &= ((Room)north).Doors.HasFlag(DoorPlacement.south);
            }
        }
        else
        {
            BaseRoom north = BuildManager.Instance.GetRoom(pair.Key.X, pair.Key.Y + 1).Value;
            if (north != null && north is Room)
            {
                isValid &= !((Room)north).Doors.HasFlag(DoorPlacement.north);
            }
        }
        //---------------------------------------------------------------------------------------------

        //----------East -> Room in East direction needs a door in west direction -> if no East door, door in East directions musnt have a west door-----------------------------------------------------------------------------------
        if (rc.Room.Doors.HasFlag(DoorPlacement.east))
        {
            BaseRoom north = BuildManager.Instance.GetRoom(pair.Key.X +1 , pair.Key.Y).Value;
            if (north != null && north is Room)
            {
                isValid &= ((Room)north).Doors.HasFlag(DoorPlacement.west);
            }
        }
        else
        {
            BaseRoom north = BuildManager.Instance.GetRoom(pair.Key.X + 1, pair.Key.Y).Value;
            if (north != null && north is Room)
            {
                isValid &= !((Room)north).Doors.HasFlag(DoorPlacement.west);
            }
        }


        //----------South -> Room in South direction needs a door in North direction -> if no South door, door in South directions musnt have a North door-----------------------------------------------------------------------------------

        if (rc.Room.Doors.HasFlag(DoorPlacement.south))
        {
            BaseRoom north = BuildManager.Instance.GetRoom(pair.Key.X, pair.Key.Y - 1).Value;
            if (north != null && north is Room)
            {
                isValid &= ((Room)north).Doors.HasFlag(DoorPlacement.north);
            }
        }
        else
        {
            BaseRoom north = BuildManager.Instance.GetRoom(pair.Key.X, pair.Key.Y - 1).Value;
            if (north != null && north is Room)
            {
                isValid &= !((Room)north).Doors.HasFlag(DoorPlacement.north);
            }
        }
        if (rc.Room.Doors.HasFlag(DoorPlacement.west))
        {
            BaseRoom north = BuildManager.Instance.GetRoom(pair.Key.X -1 , pair.Key.Y).Value;
            if (north != null && north is Room)
            {
                isValid &= ((Room)north).Doors.HasFlag(DoorPlacement.east);
            }
        }
        else
        {
            BaseRoom north = BuildManager.Instance.GetRoom(pair.Key.X -1, pair.Key.Y).Value;
            if (north != null && north is Room)
            {
                isValid &= !((Room)north).Doors.HasFlag(DoorPlacement.east);
            }
        }

        return isValid;
    }
}

