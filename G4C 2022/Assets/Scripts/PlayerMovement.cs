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
    Animator anim;

    Vector2 originalPos;

    bool inputEnabled = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        groundChecker = GetComponent<PlayerGroundChecker>();
        ladderMovement = GetComponent<LadderMovement>();
        originalPos = new Vector2(-2, -1.5f);
        Invoke(nameof(EnableInput), 5f);
        anim = GetComponent<Animator>();
        Physics2D.IgnoreLayerCollision(8, 12);
    }

    void Update()
    {
        ResetPlayerIfFallen();
        if (inputEnabled)
        {
            GetInput();
        }

        if (transform.position.y >= 12 && !GameMetrics.Instance.highAltitudeTriggered)
        {
            GameMetrics.Instance.highAltitudeTriggered = true;
            GameMetrics.Instance.UpdateUnlocks();
        }
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
        anim.SetBool("Run", inputX != 0);
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
        anim.SetTrigger("Jump");
    }

    void EnableInput()
    {
        inputEnabled = true;
    }
}
