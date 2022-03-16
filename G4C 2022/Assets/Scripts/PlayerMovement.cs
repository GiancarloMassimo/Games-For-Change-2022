using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;

    float inputX;
    Rigidbody2D rb;
    PlayerGroundChecker groundChecker;
    LadderMovement ladderMovement;

    Vector2 originalPos;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundChecker = GetComponent<PlayerGroundChecker>();
        ladderMovement = GetComponent<LadderMovement>();
        originalPos = transform.position;
    }

    void Update()
    {
        ResetPlayerIfFallen();
        GetInput();
    }

    void FixedUpdate()
    {
        if (!ladderMovement.Climbing)
        {
            rb.velocity = new Vector2(inputX * speed, rb.velocity.y);
        }
    }

    void GetInput()
    {
        inputX = Input.GetAxisRaw("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && groundChecker.Grounded)
        {
            Jump();
        }
    }

    void ResetPlayerIfFallen()
    {
        if (transform.position.y < -10)
        {
            rb.velocity = Vector2.zero;
            transform.position = originalPos;
        }
    }

    void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
