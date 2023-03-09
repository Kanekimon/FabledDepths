using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleCameraMover : MonoBehaviour
{
    public float MoveSpeed;

    public Camera cam;
    public float maxZoom = 5;
    public float minZoom = 20;
    public float sensitivity = 1;
    public float speed = 30;
    float targetZoom;

    // Start is called before the first frame update
    void Start()
    {
        targetZoom = cam.orthographicSize;
    }

    // Update is called once per frame
    void Update()
    {
        float mult = Input.GetKey(KeyCode.LeftShift) ? 2 : 1;

        float moveSpeed = Time.deltaTime * MoveSpeed * mult * (targetZoom/2);

        if (Input.GetKey(KeyCode.W))
        {
            Camera.main.transform.Translate(Vector3.up * moveSpeed);
        }
        if (Input.GetKey(KeyCode.S))
        {
            Camera.main.transform.Translate(Vector3.down * moveSpeed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            Camera.main.transform.Translate(Vector3.left * moveSpeed);
        }
        if (Input.GetKey(KeyCode.D))
        {
            Camera.main.transform.Translate(Vector3.right * moveSpeed);
        }

        targetZoom -= Input.mouseScrollDelta.y * sensitivity;
        targetZoom = Mathf.Clamp(targetZoom, maxZoom, minZoom);
        float newSize = Mathf.MoveTowards(cam.orthographicSize, targetZoom, speed * Time.deltaTime);
        cam.orthographicSize = newSize;


    }
}
