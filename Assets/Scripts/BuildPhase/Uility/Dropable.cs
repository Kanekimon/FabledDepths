using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dropable : MonoBehaviour
{
    bool isRegistered;

    private void OnMouseOver()
    {
        if(DragDropManager.Instance.CurrentlyDragging != null)
        {
            DragDropManager.Instance.RegisterDropable(this);
            Debug.Log("Draggable Is Over");
            isRegistered = true;
        }
    }

    private void OnMouseExit()
    {
        if (isRegistered)
        {
            DragDropManager.Instance.UnregisterDropable(this);
            isRegistered = false;
        }
    }
}
