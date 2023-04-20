using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorHandler : MonoBehaviour
{

    public DoorPlacement Placement;
    bool isOver = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (isOver)
        {
            if (Input.GetMouseButtonDown(0))
                RoomEditorManager.Instance.ToggleDoor(Placement, true);

            if (Input.GetMouseButtonDown(1))
                RoomEditorManager.Instance.ToggleDoor(Placement, false);
        }
    }

    private void OnMouseOver()
    {
        if (!isOver)
            isOver = true;
    }

    private void OnMouseExit()
    {
        if(isOver)
            isOver = false;
    }
}
