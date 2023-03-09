using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DragDropManager : Singleton<DragDropManager>
{
    public Draggable CurrentlyDragging;
    public Dropable CurrentlyOver;
    public Vector3 originialPos;
    public Camera Cam;

    private void Update()
    {
        if(CurrentlyDragging != null)
        {
            Vector3 mousepos = Cam.ScreenToWorldPoint(Input.mousePosition); //- new Vector3(BuildManager.Instance.Settings.RoomWidth/2, BuildManager.Instance.Settings.RoomHeight/2,0);
            CurrentlyDragging.transform.position = new Vector3(mousepos.x, mousepos.y, originialPos.z);
        }
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
}

