using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceholderClickHandler : MonoBehaviour
{
    private void OnMouseDown()
    {

        BuildManager.Instance.RegisterRoom(this);

    }

}
