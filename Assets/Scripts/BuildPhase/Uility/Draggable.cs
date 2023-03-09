using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draggable : MonoBehaviour
{
    [SerializeField] bool isDragging = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnMouseDrag()
    {
        if (!isDragging && BuildManager.Instance.BuildMode == BuildMode.Move)
        {
            isDragging = true;
            DragDropManager.Instance.RegisterDraggable(this);
        }
    }

    private void OnMouseUp()
    {
        if (isDragging)
        {
            isDragging = false;
            DragDropManager.Instance.UnregisterDraggable(this);
        }
    }

    public void ForceToDrag()
    {
        isDragging = true;
        DragDropManager.Instance.RegisterDraggable(this);
    }

}
