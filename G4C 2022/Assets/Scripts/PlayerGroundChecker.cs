using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGroundChecker : MonoBehaviour
{
    [SerializeField] Transform groundCheckPosition;
    [SerializeField] Vector2 groundCheckBoxSize;
    [SerializeField] LayerMask groundLayer;

    public bool Grounded { get; private set; }

    private void Update()
    {
        CheckGroundCollision();
    }

    void CheckGroundCollision()
    {
        if (Physics2D.OverlapBox(groundCheckPosition.position, groundCheckBoxSize, 0, groundLayer))
        {
            Grounded = true;
        }
        else
        {
            Grounded = false;
        }
    }

    public bool CheckGroundCollision(LayerMask layer)
    {
        return Physics2D.OverlapBox(groundCheckPosition.position, groundCheckBoxSize, 0, layer);
    }

}
