using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/* 
 * I will refactor this later :P
 * I promise...
 */

public class LadderMovement : MonoBehaviour
{
    [SerializeField] LayerMask ladderLayer;
    [SerializeField] float climbSpeed;
    [SerializeField] float maxHorizontalSpeed;

    public bool Climbing { get => isClimbing; }

    const int GroundLayer = 3;
    const int PlayerLayer = 8;

    PlayerGroundChecker groundChecker;
    Rigidbody2D rb;
    bool isOnLadder = false;
    bool isClimbing = false;

    float verticalInput;
    float normalGravityScale;

    void Start()
    {
        groundChecker = GetComponent<PlayerGroundChecker>();
        rb = GetComponent<Rigidbody2D>();
        normalGravityScale = rb.gravityScale;
    }


    void Update()
    {
        if (groundChecker.CheckGroundCollision(ladderLayer))
        {
            isOnLadder = true;
        }
        else
        {
            isOnLadder = false;
            isClimbing = false;
        }

        if (Input.GetAxisRaw("Vertical") != 0 && isOnLadder && (verticalInput == 0 || groundChecker.Grounded))
        {
            isClimbing = true;
        }

        verticalInput = Input.GetAxisRaw("Vertical");


        /*
         * This conditional is a clusterfuck, but it late so I'll just explain here for future me:
         * 
         * If the player is on a ladder, and they're pressing some vertical input, then you need to start climbing.
         * But there's a couple of special cases:
         * 1) if the ladder is above the player, dont let them go down (through the ground)
         * (This case only applies if there is not another ladder connected below that ladder, hence the extra bool in that condition)
         * 2) if the ladder is below the player, dont let them go up (climb on nothing) 
         * 
         * Removing those two special cases won't break the game because the collider will prevent anything bad from happening,
         * but it'll cause some jitter
         */

        if (isClimbing && verticalInput != 0)
        {
            bool ladderIsAbovePlayer = Physics2D.OverlapBox(transform.position, new Vector2(.25f, 0.05f), 0, ladderLayer);
            bool isThereALadderBelowPlayer = Physics2D.OverlapBox((Vector2)transform.position + Vector2.down, new Vector2(.25f, 0.05f), 0, ladderLayer);

            if (groundChecker.Grounded && verticalInput < 0 && ladderIsAbovePlayer && !isThereALadderBelowPlayer)
            {
                isClimbing = false;
            }
            if (groundChecker.Grounded && verticalInput > 0 && !ladderIsAbovePlayer)
            {
                isClimbing = false;
            }
        }

        if (isClimbing)
        {
            Physics2D.IgnoreLayerCollision(PlayerLayer, GroundLayer, true);
            rb.gravityScale = 0f;
        }
        else
        {
            rb.gravityScale = normalGravityScale;
            Physics2D.IgnoreLayerCollision(PlayerLayer, GroundLayer, false);
        }
    }

    void FixedUpdate()
    {
        if (isClimbing)
        {
            rb.velocity = new Vector2(
                Input.GetAxisRaw("Horizontal") * maxHorizontalSpeed, 
                verticalInput * climbSpeed
                );
        }
    }
}
