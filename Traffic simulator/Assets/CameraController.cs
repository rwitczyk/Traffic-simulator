using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    /* public float zoomSpeed = 1;
     public float scrollSpeed = 1;

     [Header("Speed")]


     public float rotationSpeed = 15;
     public float scrollKeyboardSpeed = 15;*/

    [Header("Limits")]
    public float minZoom = 40;
    public float maxZoom = 20;
    public Vector3 minPosition = new Vector3(0, 100,-350);
    public Vector3 maxPosition = new Vector3(400, 400,350);
    public Vector3 dd = new Vector3(10, 10, 10);


    private float scrollSpeedUp = 0;

    void Start()
    {
    }

    void Update ()
    {
        // Przesuwanie kamery myszka i klawiatura
        if (Input.GetKey(KeyCode.UpArrow) )
        {
            transform.Translate(new Vector3(0, 1, 1));
        }

        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, -1, -1));
        }

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Translate(new Vector3(-1, 0, 0));
        }

        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Translate(new Vector3(1, 0, 0));
        }

        
        //scroll myszki
        if ((Input.mouseScrollDelta.y / 10) > 0)
        {
            gameObject.transform.Translate(new Vector3(0, 0, 5));
        }
        if ((Input.mouseScrollDelta.y / 10) < 0)
        {
            gameObject.transform.Translate(new Vector3(0, 0, -5));
        }

        /*
        transform.position=
        Vector3 direction = Vector3.zero;

        if (Input.GetKey(KeyCode.UpArrow))
        {
            direction = -Vector3.left;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
                direction = -Vector3.left;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
                direction = -Vector3.left;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
                direction = -Vector3.left;
        }
        */
    }
}
