using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    Camera cam;

    void Start()
    {
        cam = Camera.main;
        Cursor.visible = false;
    }

    void Update()
    {
        transform.position = (Vector2)cam.ScreenToWorldPoint(Input.mousePosition);
    }
}
