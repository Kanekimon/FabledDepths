using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        if(Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            Vector3 moveVector = new Vector3();
            moveVector.x += Input.GetAxis("Horizontal") * Speed * Time.deltaTime;
            moveVector.y += Input.GetAxis("Vertical") * Speed * Time.deltaTime;
            this.transform.position += moveVector;
        }


        

    }
}
