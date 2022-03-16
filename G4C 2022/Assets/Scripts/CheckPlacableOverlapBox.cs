using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlacableOverlapBox : MonoBehaviour
{
    [SerializeField] Transform topLeft, bottomRight;
    [SerializeField] LayerMask layerMask;

    Vector2 a, b;

    void Start()
    {
        a = topLeft.position;
        b = bottomRight.position;

        if (a.x > b.x || a.y < b.y)
        {
            Debug.LogWarning("Overlap Box Coordinates incorrectly placed");
        }
    }

    public bool CheckOverlap()
    {
        a = topLeft.position;
        b = bottomRight.position;

        Vector2 origin = new Vector2((a.x + b.x) / 2f, (a.y + b.y) / 2f);
        Vector2 size = new Vector2(b.x - a.x, a.y - b.y);

        return Physics2D.OverlapBox(origin, size, 0, layerMask);
    }

}
